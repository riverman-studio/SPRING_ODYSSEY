using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class nodeBehaviour : MonoBehaviour
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
    



    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _vfx = gameObject.GetComponent<VisualEffect>();
        _gpConnector = connectionManager.GetComponent<gpConnector>();
        _collisionCheck = _gpConnector.collisionCheck.gameObject;
        Reset();
        _vfx.SetFloat("alpha", 0.0f);
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
    }

    void Update()
    {

        if (!_gpActivated)
            return;
        Transform cameraTransform = Camera.main.transform;
        RaycastHit HitInfo;
        RaycastHit[] hits;
        bool bTouching = false;
        Vector3 touchPos = new Vector3();
        //if(_state != ConnectionState.Reaching)
        if (Input.touches.Length > 0)
        {
            bTouching = true;
            touchPos = Input.touches[0].position;
        }
        else if (Input.GetMouseButton(0))
        {
            bTouching = true;
            touchPos = Input.mousePosition;
            touchPos.x = Screen.width / 2;
            touchPos.y = Screen.height / 2;

        }
        if (bTouching)
        {
            Vector3 fromPos = Camera.main.WorldToScreenPoint(DrawFrom.position);
            Vector3 toPos = Camera.main.WorldToScreenPoint(DrawTo.position);
            float fProg = progress(fromPos, toPos, touchPos);
            Debug.Log(fProg);

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
        }

    }

        // Update is called once per frame
    void Update2()
    {
        if (!_gpActivated)
            return;
        Transform cameraTransform = Camera.main.transform;
        RaycastHit HitInfo;
        RaycastHit[] hits;
        bool bTouching = false;
        Vector3 touchPos;
        //if(_state != ConnectionState.Reaching)
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

        if ((_state == ConnectionState.Reaching) && !bTouching)
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(0, 0, 0);
            positions[0] = DrawFrom.position;
            positions[1] = new Vector3(0, 0, 0);
            positions[1] = DrawTo.position;
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
            _state = ConnectionState.Reached;
            Destroy(_connHelper.gameObject);
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
            }

        }
        if (_state == ConnectionState.Reached)
        {
            _gpActivated = false;
            _gpConnector.crosshairHide();
            return;
        }

        if (!bTouching)
        {
            if((_state != ConnectionState.NotStarted) || (_state != ConnectionState.Reached))
            {
                Destroy(_connHelper.gameObject);
                ActivateInteraction();
            }
        }

        if (_state == ConnectionState.NotStarted)
        {
            /*_gpConnector.crosshairRed();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out HitInfo, 100.0f))
            {
                if (HitInfo.collider.gameObject.GetInstanceID() == _connHelper.gameObject.GetInstanceID())
                {
                    Renderer r = _connHelper.gameObject.GetComponent<Renderer>();
                    r.material.color = Color.yellow;
                    _state = ConnectionState.HilightedFirst;
                    _gpConnector.crosshairYellow();
                }
            }
            else
            {
                Renderer r = _connHelper.gameObject.GetComponent<Renderer>();
                r.material.color = Color.red;
                _state = ConnectionState.NotStarted;
            }*/
            _state = ConnectionState.HilightedFirst;
        }
        if ((_state == ConnectionState.HilightedFirst) && bTouching)
        {
            Destroy(_connHelper.gameObject);
            _connHelper = Instantiate(_gpConnector.connHiLite, DrawTo.position, Quaternion.identity);
            _state = ConnectionState.Activated;
        }
        if (((_state == ConnectionState.Activated) || (_state == ConnectionState.Reaching)) && bTouching)
        {
            _gpConnector.crosshairRed();
            bool bGot = false;
            hits = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, 100.0F);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.GetInstanceID() == _connHelper.gameObject.GetInstanceID())
                {
                    Renderer r = _connHelper.gameObject.GetComponent<Renderer>();
                    r.material.color = Color.yellow;
                    _state = ConnectionState.Reaching;
                    bGot = true;
                    _gpConnector.crosshairYellow();
                    //Debug.Log("in yellow");
                }
            }
            if(!bGot)
            {
                Renderer r = _connHelper.gameObject.GetComponent<Renderer>();
                r.material.color = Color.red;
                _state = ConnectionState.Activated;
            }
        }

        if((_state == ConnectionState.Activated) || (_state == ConnectionState.Reaching))
        {
            hits = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, 100.0F);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.GetInstanceID() == _collisionCheck.GetInstanceID())
                {
                    Vector3[] positions = new Vector3[2];
                    positions[0] = new Vector3(0, 0, 0);
                    positions[0] = DrawFrom.position;
                    positions[1] = new Vector3(0, 0, 0);
                    positions[1] = hit.point;
                    _lineRenderer.positionCount = positions.Length;
                    _lineRenderer.SetPositions(positions);
                    Color strandColor = _lineRenderer.material.color;
                    strandColor.a = 1.0f;
                    _lineRenderer.material.color = strandColor;
                    _lineRenderer.startColor = strandColor;
                    _lineRenderer.endColor = strandColor;
                    _lineRenderer.SetColors(strandColor, strandColor);
                    float fFactor  = progress(DrawFrom.position, DrawTo.position, hit.point);
                    if (sisterConnections.Length != 0)
                    {
                        for (int j = 0; j < sisterConnections.Length; j++)
                        {
                            sisterConnections[j].GetComponent<sisterNodeBehaviour>().drawLine(fFactor);
                        }
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
        Debug.Log(fromPt.ToString() + " " + toPt.ToString() + " " + pt.ToString());
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




    private IEnumerator coroutine = null;
    void FadeToColor(LineRenderer lr, Color toColor)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = __fadeToColor(lr, toColor);
        StartCoroutine(coroutine);
    }


    IEnumerator __fadeToColor(LineRenderer lr, Color endColor)
    {
        Color startColor = lr.startColor;
         
        float fDelta = 0.0f;
        while (fDelta < 1.0f)
        {
            Color dcol = Color.Lerp(startColor, endColor, fDelta);
            lr.SetColors(dcol, dcol);
            fDelta += (Time.deltaTime * 2.0f);
            yield return null;
        }
        yield return null;
    }


}
