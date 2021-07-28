using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class microBehaviour : MonoBehaviour
{
    LineRenderer _lineRenderer = null;
    VisualEffect _vfx = null;


    // Start is called before the first frame update
    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _vfx = gameObject.GetComponent<VisualEffect>();
        ResetPosition();
    }
    public void ResetPosition()
    {
        Transform pt1 = gameObject.transform.GetChild(0);
        Transform pt2 = gameObject.transform.GetChild(1);
        if (_lineRenderer)
        {
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            Color strandColor = _lineRenderer.material.color;
            strandColor.a = 0.0f;
            _lineRenderer.material.color = strandColor;
            _lineRenderer.startColor = strandColor;
            _lineRenderer.endColor = strandColor;
            _lineRenderer.SetColors(strandColor, strandColor);


            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(0, 0, 0);
            positions[0] = pt1.position;
            positions[1] = new Vector3(0, 0, 0);
            positions[1] = pt2.position;
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
            _lineRenderer.textureMode = LineTextureMode.Tile;
        }
    }

    public void fillTheLine(float fLevelDelay)
    {
        if(_lineRenderer)
        {
            FadeToColor(_lineRenderer, Color.white, fLevelDelay);
        }
        
    }
    private IEnumerator coroutine = null;
    void FadeToColor(LineRenderer lr, Color toColor, float fLevelDelay)
    {
        /*if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }*/
        coroutine = __fadeToColor(lr, toColor, fLevelDelay);
        StartCoroutine(coroutine);
    }
    IEnumerator __fadeToColor(LineRenderer lr, Color endColor, float fLevelDelay)
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 5f));
        Color startColor = lr.startColor;

        float fDelta = 0.0f;
        while (fDelta < 1.0f)
        {
            Color dcol = Color.Lerp(startColor, endColor, fDelta);
            dcol.a = Mathf.Lerp(startColor.a, endColor.a, fDelta);

            lr.material.color = dcol;
            lr.startColor = dcol;
            lr.endColor = dcol;
            lr.SetColors(dcol, dcol);

            fDelta += (Time.deltaTime * 1.0f);
            yield return null;
        }
        yield return null;
    }


}
