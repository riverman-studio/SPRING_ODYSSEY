using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class nodeBehaviour : templateBehaviour
{
    // Start is called before the first frame update

    enum ConnectionState { NotStarted, HilightedFirst, Activated, Reaching, Reached, Released };

    public int lineNumber = 0;
    LineRenderer _lineRenderer = null;
    VisualEffect _vfx = null;
    
    public Transform DrawFrom, DrawTo;
    public Transform connectionManager;
    
    public GameObject NextOne;
    public bool _gpActivated = false;
    public GameObject[] sisterConnections;


    private ConnectionState _state;
    private Transform _connHelper = null;
    private GameObject _collisionCheck = null;
    private gpConnector _gpConnector = null;
    private Animator masterAnimator = null;
    private float blipRate = 8.0f;

    public override LineRenderer GetLineRenderer()
    {
        return _lineRenderer;
    }


    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!_lineRenderer)
        {
            enabled = false;
            return;
        }
        _vfx = gameObject.GetComponent<VisualEffect>();
        _gpConnector = connectionManager.GetComponent<gpConnector>();
        if(_gpConnector.collisionCheck)
            _collisionCheck = _gpConnector.collisionCheck.gameObject;
        Reset();
        _vfx.SetFloat("alpha", 0.0f);

        masterAnimator = GameObject.Find("__TIMELINE").GetComponent<Animator>();



    }
    private void Start()
    {
        if (_gpActivated)
        {
            ActivateInteraction();
        }


    }
    void Reset()
    {
        _state = ConnectionState.NotStarted;
        Color strandColor = _lineRenderer.material.color;
        strandColor.a = 0.0f;
        _lineRenderer.material.color = strandColor;
        _lineRenderer.startColor = strandColor;
        _lineRenderer.endColor = strandColor;
        _lineRenderer.SetColors(strandColor, strandColor);
        if (sisterConnections.Length != 0)
        {
            for (int j = 0; j < sisterConnections.Length; j++)
            {
                sisterConnections[j].GetComponent<sisterNodeBehaviour>().Reset();
            }
        }
        _gpConnector.crosshairHide();
    }
    void ActivateInteraction()
    {
        _connHelper = Instantiate(_gpConnector.connHiLite, DrawFrom.position, Quaternion.identity);
        _gpActivated = true;
        Reset();
        _vfx.SetFloat("alpha", 1.0f);
        waveDottedLine(true);
    }

    void Update()
    {

        if (!_gpActivated)
            return;
        bool atInteractionState = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("07_InteractionLigneToComplete");

        if (!atInteractionState)
            return;

        blipRate = 3.0f;
        Transform cameraTransform = Camera.main.transform;
        RaycastHit HitInfo;
        RaycastHit[] hits;
        bool bTouching = false;
        Vector3 touchPos = new Vector3();
        touchPos.x = Screen.width / 2;
        touchPos.y = Screen.height / 2;

        if (Input.touches.Length > 0)
        {
            bTouching = true;
            touchPos = Input.touches[0].position;
        }
        else if (Input.GetMouseButton(0))
        {
            bTouching = true;
            touchPos = Input.mousePosition;
        }
        if ((_state == ConnectionState.Reached) && !bTouching)
        {
            if(_vfx)
            {
                _vfx.enabled = false;
            }
            
            _gpActivated = false;
            _gpConnector.crosshairHide();
            Destroy(_connHelper.gameObject);
            _connHelper = null;

            if (sisterConnections.Length != 0)
            {
                for (int j = 0; j < sisterConnections.Length; j++)
                {
                    sisterConnections[j].GetComponent<sisterNodeBehaviour>().drawLine(1.0f);
                }
            }

            if (NextOne)
            {
                nodeBehaviour nb = NextOne.GetComponent<nodeBehaviour>();
                nb.ActivateInteraction();
            }
            else
            {
                _gpConnector.activateEverySegment();

                GameObject timeLine = GameObject.Find("__TIMELINE");
                timeLine.GetComponent<Animator>().SetTrigger("Trigger3");
            }
        }
        if ((_state == ConnectionState.HilightedFirst) && !bTouching)
        {
            _connHelper.transform.position = DrawFrom.position;
            _state = ConnectionState.NotStarted;
            Renderer r = _connHelper.gameObject.GetComponent<Renderer>();
            r.material.color = Color.white;

            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(0, 0, 0);
            positions[0] = DrawFrom.position;
            positions[1] = new Vector3(0, 0, 0);
            positions[1] = DrawFrom.position;
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);


        }
        if ((_state == ConnectionState.NotStarted) && !bTouching)
        {
            Vector3 fromPosScrn = Camera.main.WorldToScreenPoint(DrawFrom.position);
            Vector3 toPosScrn = Camera.main.WorldToScreenPoint(DrawTo.position);
            fromPosScrn.z = 0.0f;
            toPosScrn.z = 0.0f;

            float fDistDrawLine = Vector3.Distance(fromPosScrn, toPosScrn);
            Vector3 toPos = touchPos;
            float fDist = Vector3.Distance(fromPosScrn, toPos);
            Vector3 drawDirection = (DrawFrom.position - DrawTo.position).normalized;
            float invFDot = 1.0f - Mathf.Abs(Vector3.Dot(drawDirection, cameraTransform.forward));
            float distFactor = 1.0f - Mathf.Clamp01(fDist / fDistDrawLine);
            blipRate = Mathf.Lerp(3.0f, 16.0f, Mathf.Sqrt(invFDot * distFactor));

        }
        if (bTouching)
        {
            if (_state == ConnectionState.NotStarted)
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                if (Physics.Raycast(ray, out HitInfo, 100.0f))
                //if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out HitInfo, 100.0f))
                {
                    if (HitInfo.collider.gameObject.GetInstanceID() == _connHelper.gameObject.GetInstanceID())
                    {
                        _state = ConnectionState.HilightedFirst;
                    }
                }
            }


            if(_state == ConnectionState.HilightedFirst)
            {
                Vector3 fromPos = Camera.main.WorldToScreenPoint(DrawFrom.position);
                Vector3 toPos = Camera.main.WorldToScreenPoint(DrawTo.position);
                float fProg = progress(fromPos, toPos, touchPos);
                if (fProg > 0.95)
                {
                    _state = ConnectionState.Reached;
                    fProg = 1.0f;
                    waveDottedLine(false);

                }
 
                    Vector3 midPt = Vector3.Lerp(DrawFrom.position, DrawTo.position, fProg);

                    Vector3[] positions = new Vector3[2];
                    positions[0] = new Vector3(0, 0, 0);
                    positions[0] = DrawFrom.position;
                    positions[1] = new Vector3(0, 0, 0);
                    positions[1] = midPt;
                    _lineRenderer.positionCount = positions.Length;
                    _lineRenderer.SetPositions(positions);
                    Color strandColor = _lineRenderer.material.color;

                    strandColor.a = 1.0f;
                    _lineRenderer.material.color = strandColor;
                    _lineRenderer.startColor = strandColor;
                    _lineRenderer.endColor = strandColor;
                    _lineRenderer.SetColors(strandColor, strandColor);
                    _connHelper.transform.position = midPt;
                    if (sisterConnections.Length != 0)
                    {
                        for (int j = 0; j < sisterConnections.Length; j++)
                        {
                            sisterConnections[j].GetComponent<sisterNodeBehaviour>().drawLine(fProg);
                        }
                    }

            }

        }

    }

    float progress(Vector3 fromPt, Vector3 toPt, Vector3 pt)
    {
        fromPt.z = 0.0f;
        toPt.z = 0.0f;
        pt.z = 0.0f;
        /* fromPt = Camera.main.WorldToScreenPoint(fromPt);
         toPt = Camera.main.WorldToScreenPoint(toPt);
         pt = Camera.main.WorldToScreenPoint(pt);*/
        return GetApproachRate(fromPt, toPt, pt);
    }

    float GetApproachRate(Vector3 line_start, Vector3 line_end, Vector3 point)
    {
        
        Vector3 line_direction = line_end - line_start;
        float line_length = line_direction.magnitude;
        line_direction.Normalize();
        float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
        float progressRate = project_length / Vector3.Distance(line_start, line_end);
        return progressRate;
    }
    private Coroutine __corDottedWave = null;
    void waveDottedLine(bool activateWave)
    {
        if (activateWave)
            __corDottedWave = StartCoroutine(__waveDotted());
        else if (__corDottedWave != null)
        {
            StopCoroutine(__corDottedWave);
            __corDottedWave = null;
        }
    }
    IEnumerator __waveDotted()
    {

        Renderer r = _connHelper.gameObject.GetComponent<Renderer>();

        float fTime = 0.0f;
        for(; ; )
        {
            if (!_gpActivated)
            {

                yield return null;
            }
                

            fTime += (Time.deltaTime * blipRate);
            float fAlpha = Mathf.Sin(fTime);
            fAlpha = 0.5f + (fAlpha / 2.0f);
            _vfx.SetFloat("alpha", fAlpha);


            if (_state == ConnectionState.HilightedFirst)
            {
                r.material.color = Color.white;
            }
            else
            {
                Color helperCol = r.material.color;
                helperCol.a = fAlpha;
                r.material.color = helperCol;
            }
            yield return null;
        }
        yield return null;
    }



}
