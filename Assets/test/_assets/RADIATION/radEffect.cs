using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radEffect : MonoBehaviour
{
    public Transform radiationDirection;
    public Transform radiationAsset;
    public Transform preRadiationAsset;
    public BoxCollider spawnSpot;
    public float radTargettingSqFactor = 4.0f;
    public float radTargettingFocus = 0.0f;
    //public float fadeInSpeed = 2.0f;
    public float fadeOutSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("makeBalls");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator makeBalls()
    {
        yield return new WaitForSeconds(3.0f);
        //for (int i=0; i<10; i++)
        for(; ; )
        {
            Vector3 min = spawnSpot.bounds.min;
            Vector3 max = spawnSpot.bounds.max;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.8f));

            Vector3 vecPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            Transform lastPreRad = Instantiate(preRadiationAsset, vecPos, Quaternion.identity);
            StartCoroutine(__formRadiation(lastPreRad));
            //yield return new WaitForSeconds(5.0f);
        }
        yield return null;
    }


    IEnumerator __formRadiation(Transform lastPreRad)
    {
        BoxCollider preRadBoxCollider = lastPreRad.GetComponent<BoxCollider>();
        Vector3 min = preRadBoxCollider.bounds.min;
        Vector3 max = preRadBoxCollider.bounds.max;
        float fShakeTime = 0.0f;
        float fMaxShakeTime = Random.Range(1.0f, 2.0f);
        while(fShakeTime < fMaxShakeTime)
        {
            Vector3 vecPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            lastPreRad.position = vecPos;
            yield return new WaitForSeconds(0.05f);
            fShakeTime += 0.05f;
        }

        Quaternion quat;
        Vector3 eulerRot = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        quat = Quaternion.Euler(eulerRot); 
        Transform lastBang = Instantiate(radiationAsset, lastPreRad.position, quat);
        float scale = 0.0f;
        for (; ; )
        {
            scale = scale + (Time.deltaTime * 3.0f);
            lastBang.localScale = new Vector3(scale, scale, scale);

            if (scale > 0.3f)
                break;
            else
                yield return null;
        }

        fShakeTime = 0.0f;
        fMaxShakeTime = Random.Range(0.5f, 1.0f);
        while (fShakeTime < fMaxShakeTime)
        {
            Vector3 vecPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            Vector3 eulerDelta = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            eulerRot += eulerDelta;
            quat = Quaternion.Euler(eulerRot);
            lastBang.position = vecPos;
            lastBang.rotation = quat;
            yield return new WaitForSeconds(0.05f);
            fShakeTime += 0.05f;
        }
        //yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
        Animator animator = lastBang.gameObject.GetComponentInChildren<Animator>();
        animator.enabled = true;
        StartCoroutine(__fadeIt2(lastBang));
        GameObject.Destroy(lastPreRad.gameObject);

    }
    IEnumerator __fadeIt2(Transform lastBang)
    {
        Vector3 vCenter = radiationDirection.position;
        Transform[] strandTx = lastBang.gameObject.GetComponentsInChildren<Transform>();

        Vector3 vDirToPlant = new Vector3();
        bool Faded = false;
        float alpha = 1.0f;

        float fStartFade = 0.75f;
        float fEndFade = 0.25f;
        float fEndFade2 = 1.0f;

        vDirToPlant = (vCenter - lastBang.position).normalized;
        yield return new WaitForSeconds(0.3f);


        float[] fadeSpeed = new float[strandTx.Length +1 ];
        float[] alphas = new float[strandTx.Length +1 ];
        for (int i = 0; i < strandTx.Length; i++)
        {
            if (lastBang.gameObject.GetInstanceID() == strandTx[i].gameObject.GetInstanceID())
                continue;
            fadeSpeed[i] = 1.0f;
            Vector3 strandDirection = (strandTx[i].position - lastBang.position).normalized;
            float factor = Vector3.Dot(strandDirection, vDirToPlant);
            fadeSpeed[i] =  1.0f - factor;
            fadeSpeed[i] = fadeSpeed[i] + radTargettingFocus;
            //fadeSpeed[i] = Mathf.Sqrt(Mathf.Sqrt(fadeSpeed[i]));
            fadeSpeed[i] = Mathf.Pow(fadeSpeed[i], radTargettingSqFactor);
        }

        while (! Faded)
        {
            //
            Faded = true;
            for (int i = 0; i < strandTx.Length; i++)
            {
                if (lastBang.gameObject.GetInstanceID() == strandTx[i].gameObject.GetInstanceID())
                    continue;
                if (fadeSpeed[i] == 0.0f)
                    continue;

                Renderer rnderer = strandTx[i].gameObject.GetComponent<Renderer>();
                Color strandColor = rnderer.material.color;
                if (strandColor.a > 0.01f)
                    Faded &= false;
                else
                    Faded &= true;

                strandColor.a = strandColor.a - (fadeSpeed[i] * Time.deltaTime * fadeOutSpeed);
                alphas[i] = strandColor.a;
                strandColor.r = 2;
                rnderer.material.color = strandColor;

            }
            yield return null;
        }

        Faded = false;
        while (!Faded)
        {
            //
            Faded = true;
            for (int i = 0; i < strandTx.Length; i++)
            {
                if (lastBang.gameObject.GetInstanceID() == strandTx[i].gameObject.GetInstanceID())
                    continue;
                if (fadeSpeed[i] != 0.0f)
                    continue;

                Renderer rnderer = strandTx[i].gameObject.GetComponent<Renderer>();
                Color strandColor = rnderer.material.color;
                if (strandColor.a > 0.01f)
                    Faded &= false;
                else
                    Faded &= true;

                strandColor.a = strandColor.a - (Time.deltaTime * 10.0f);
                alphas[i] = strandColor.a;
                rnderer.material.color = strandColor;

            }
            yield return null;
        }




        GameObject.Destroy(lastBang.gameObject);
        lastBang = null;

        yield return null;
    }



    IEnumerator __fadeIt(Transform lastBang)
    {
        Vector3 vCenter = new Vector3(0, 2, 0);
        Transform[] strandTx = lastBang.gameObject.GetComponentsInChildren<Transform>();
        bool StillFading = true;
        float alpha = 1.0f;

        float fStartFade = 0.75f;
        float fEndFade = 0.25f;
        float fEndFade2 = 1.0f;

        while (StillFading)
        {
            alpha = alpha - (fadeOutSpeed * Time.deltaTime);
            for (int i = 0; i < strandTx.Length; i++)
            {
                if (lastBang.gameObject.GetInstanceID() == strandTx[i].gameObject.GetInstanceID())
                    continue;
                Renderer rnderer = strandTx[i].gameObject.GetComponent<Renderer>();
                Color strandColor = rnderer.material.color;
                if (strandColor.a > 0.0001f)
                    StillFading &= true;

                //calculate distance between strand and center
                float fDist = Vector3.Distance(vCenter, strandTx[i].position);
                float fFactor = 1.0f; 
                if (fDist < 0.75f)
                {
                    fFactor = (fDist - fEndFade) / (fStartFade - fEndFade);
                    fFactor = Mathf.Clamp01(fFactor);
                }
                else
                {
                    fFactor = (fEndFade2 - fDist) / (fEndFade2 - fStartFade);
                    fFactor = Mathf.Clamp01(fFactor);
                }

                strandColor.a = fFactor;
                rnderer.material.color = strandColor;

            }
            yield return null;
        }
        GameObject.Destroy(lastBang.gameObject);
        lastBang = null;

        yield return null;
    }
}
