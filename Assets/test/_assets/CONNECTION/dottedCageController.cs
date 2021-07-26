using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class dottedCageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void setColor(float fAlpha)
    {
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Transform levelChild = gameObject.transform.GetChild(j);
            for (int i = 0; i < levelChild.childCount; i++)
            {
                Transform cageSegment = levelChild.GetChild(i);
                VisualEffect vfx = cageSegment.GetComponent<VisualEffect>();
                if (vfx)
                {
                    //mb.fillTheLine((float)j);
                    vfx.SetFloat("alpha", fAlpha);
                }
            }
        }
    }

    void FadeIn()
    {
        StartCoroutine(__fadeIn());
    }
    IEnumerator __fadeIn()
    {
        float fAlpha = 0.0f;
        while(fAlpha < 1.0f)
        {
            fAlpha += (Time.deltaTime * 1.0f);
            setColor(fAlpha);
        }
        setColor(1.0f);
        yield return null;             
    }

    void FadeOut()
    {
        StartCoroutine(__fadeOut());
    }
    IEnumerator __fadeOut()
    {
        float fAlpha = 1.0f;
        while (fAlpha > 0.0f)
        {
            fAlpha -= (Time.deltaTime * 1.0f);
            setColor(fAlpha);
        }
        setColor(0.0f);
        yield return null;
    }
}
