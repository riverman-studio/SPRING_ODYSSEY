using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radiationBoum : MonoBehaviour
{
    public string boumState;
    public Transform radiationDirection;
    public float radTargettingFocus = 0.8f;
    public float radTargettingSqFactor = 5.0f;
    public float fadeOutSpeed = 0.1f;
    public float maxFadeTime = 4.0f;


    // Start is called before the first frame update
    List<MeshRenderer> seeds = new List<MeshRenderer>();
    List<TrailRenderer> trails = new List<TrailRenderer>();
    List<Renderer> strandTx = new List<Renderer>();
    Animator animator = null;
    bool fadeKicked = false;
    Gradient gradient = null;
    void Start()
    {
       /* gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );*/
        


        animator = GetComponent<Animator>();
        MeshRenderer [] mshRenderer = transform.GetComponentsInChildren<MeshRenderer>();
        for (int i=0; i<mshRenderer.Length; i++)
        {
            if(mshRenderer[i].gameObject.name.StartsWith("radioSeed"))
            {
                seeds.Add(mshRenderer[i]);
                //strandTx.Add(mshRenderer[i].gameObject.transform);
                strandTx.Add(mshRenderer[i] as Renderer);
            }
        }
        TrailRenderer[]  trailRenderer = transform.GetComponentsInChildren<TrailRenderer>();
        for (int i = 0; i < trailRenderer.Length; i++)
        {
            if (trailRenderer[i].gameObject.name.StartsWith("SeedTrail"))
            {
                trails.Add(trailRenderer[i]);
                //strandTx.Add(mshRenderer[i].gameObject.transform);
                strandTx.Add(trailRenderer[i] as Renderer);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( animator.GetCurrentAnimatorStateInfo(0).IsName("Boum"))
        {
            if(!fadeKicked)
            {
                StartCoroutine(__kickFade());
                fadeKicked = true;
            }
        }
    }

    IEnumerator __kickFade()
    {
        Transform lastBang = gameObject.transform;
        Vector3 vCenter = radiationDirection.position;
        Vector3 vDirToPlant = (vCenter - lastBang.position).normalized;
        bool Faded = false;
        yield return new WaitForSeconds(0.3f);

        float[] fadeSpeed = new float[strandTx.Count + 1];
        float[] alphas = new float[strandTx.Count + 1];
        for (int i = 0; i < strandTx.Count; i++)
        {
            if (lastBang.gameObject.GetInstanceID() == strandTx[i].gameObject.GetInstanceID())
                continue;
            fadeSpeed[i] = 1.0f;
            Vector3 strandDirection = (strandTx[i].transform.position - lastBang.position).normalized;
            float factor = Vector3.Dot(strandDirection, vDirToPlant);
            fadeSpeed[i] = 1.0f - factor;
            fadeSpeed[i] = fadeSpeed[i] + radTargettingFocus;
            //fadeSpeed[i] = Mathf.Sqrt(Mathf.Sqrt(fadeSpeed[i]));
            fadeSpeed[i] = Mathf.Pow(fadeSpeed[i], radTargettingSqFactor);
            alphas[i] = 1.0f;
        }
        float fTotalTime = 0.0f;
        while(true)
        {
            fTotalTime += Time.deltaTime;
            for (int i = 0; i < strandTx.Count; i++)
            {
                Color fadeColor = Color.white;
                Renderer rnderer = strandTx[i];
                fadeColor.a = alphas[i];
                setColor(rnderer, fadeColor);
                float fAlphs = (Time.deltaTime * fadeOutSpeed * fadeSpeed[i]);
                alphas[i] -= Mathf.Clamp01(fAlphs);
            }
            if (fTotalTime > maxFadeTime)
                break;
            yield return null;
        }

        for (int i = 0; i < strandTx.Count; i++)
        {
            GameObject.Destroy(strandTx[i].gameObject);
        }

        yield return null;
    }

    Color getAlpha(Renderer rnderer)
    {
       /*if (rnderer.GetType() == typeof(TrailRenderer))
        {
            TrailRenderer trailRnd = rnderer as TrailRenderer;
            return trailRnd.startColor;
        }
        else if (rnderer.GetType() == typeof(MeshRenderer))
        {
            return rnderer.material.color;
        }*/
        return Color.white;
    }
    void setColor(Renderer rnderer, Color strandColor)
    {
        //strandColor.a = 0.5f;
        strandColor.a = Mathf.Clamp01(strandColor.a);
        if (rnderer.GetType() == typeof(MeshRenderer))
        {
            rnderer.material.color = strandColor;
        }
        else if (rnderer.GetType() == typeof(TrailRenderer))
        {
            TrailRenderer trailRnd = rnderer as TrailRenderer;
            rnderer.material.SetColor("_BaseColor", strandColor);

            //trailRnd.colorGradient = gradient;
            /*trailRnd.colorGradient.alphaKeys[0].alpha = strandColor.a;
            trailRnd.colorGradient.alphaKeys[1].alpha = strandColor.a;*/
            //trailRnd.material.color = strandColor;
            /*trailRnd.startColor = strandColor;
            trailRnd.endColor = strandColor;*/
            //trailRnd.SetColors(strandColor, strandColor);
        }
    }
}
