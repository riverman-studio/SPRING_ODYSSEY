using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sisterNodeBehaviour : templateBehaviour
{
    public int lineNumber=0;
    public Transform parentNode;
    LineRenderer _lineRenderer = null;
    public Transform DrawFrom, DrawTo;
    public override LineRenderer GetLineRenderer()
    {
        return _lineRenderer;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!_lineRenderer)
        {
            enabled = false;
            return;
        }
        Reset();
    }
    public void Reset()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        Color strandColor = _lineRenderer.material.color;
        strandColor.a = 0.0f;
        _lineRenderer.material.color = strandColor;
        _lineRenderer.startColor = strandColor;
        _lineRenderer.endColor = strandColor;
        _lineRenderer.SetColors(strandColor, strandColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void drawLine(float fProgress)
    {
        Vector3 midLine = Vector3.Lerp(DrawFrom.position, DrawTo.position, fProgress);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(0, 0, 0);
        positions[0] = DrawFrom.position;
        positions[1] = new Vector3(0, 0, 0);
        positions[1] = midLine;
        _lineRenderer.positionCount = positions.Length;
        _lineRenderer.SetPositions(positions);
        Color strandColor = _lineRenderer.material.color;
        strandColor.a = 1.0f;
        _lineRenderer.material.color = strandColor;
        _lineRenderer.startColor = strandColor;
        _lineRenderer.endColor = strandColor;
        _lineRenderer.SetColors(strandColor, strandColor);

    }
        

    public void fillTheLine()
    {
        FadeToColor(_lineRenderer, Color.white);
    }
    private IEnumerator coroutine = null;
    void FadeToColor(LineRenderer lr, Color toColor)
    {
        if (coroutine != null)
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
            dcol.a = Mathf.Lerp(startColor.a, endColor.a, fDelta);
            
            lr.material.color = dcol;
            lr.startColor = dcol;
            lr.endColor = dcol;
            lr.SetColors(dcol, dcol);

            fDelta += (Time.deltaTime * 0.5f);
            yield return null;
        }
        yield return null;
    }
}
