using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class templateBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual LineRenderer GetLineRenderer()
    {
        return null;
    }
    public void fillTheLine(float fLevelDelay)
    {
        if (GetLineRenderer())
        {
            FadeToColor(GetLineRenderer(), Color.white, fLevelDelay);
        }
    }
    public void unFillTheLine(float fLevelDelay)
    {
        if (GetLineRenderer())
        {
            FadeToColor(GetLineRenderer(), Color.clear, fLevelDelay);
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
        yield return new WaitForSeconds(fLevelDelay + Random.Range(0.5f, 3f));
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
