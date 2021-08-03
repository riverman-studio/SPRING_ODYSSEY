using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class microBehaviour : templateBehaviour
{
    LineRenderer _lineRenderer = null;
    VisualEffect _vfx = null;

    public override LineRenderer GetLineRenderer()
    {
        return _lineRenderer;
    }

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




}
