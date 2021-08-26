using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    List<int> seedList = new List<int>();
    public Color radColor;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.identity;
    }
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
            //If the GameObject's name matches the one you suggest, output this message in the console
        if(!seedList.Contains(collision.gameObject.GetInstanceID()))
        {
            StartCoroutine(__changeColor(collision.gameObject));
            seedList.Add(collision.gameObject.GetInstanceID());
        }
        
    }

    void OnCollisionStay(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        //If the GameObject's name matches the one you suggest, output this message in the console
        if (!seedList.Contains(collision.gameObject.GetInstanceID()))
        {
            StartCoroutine(__changeColor(collision.gameObject));
            seedList.Add(collision.gameObject.GetInstanceID());
        }

    }


    
    /*void OnCollisionStay(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject

            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Do something here");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Do something here");
    }*/

    IEnumerator __changeColor(GameObject radSeed)
    {
        Renderer seedRenderer = radSeed.GetComponent<Renderer>();

        Color startColor = seedRenderer.material.color;
        
        float fDelta = 0.0f;
        while(fDelta < 1.0f)
        {
            fDelta += Time.deltaTime * 1.0f;
            Color fadingCOlor = Color.Lerp(startColor, radColor, fDelta);
            fadingCOlor.a = seedRenderer.material.color.a;
            seedRenderer.material.SetColor("_BaseColor", fadingCOlor);
            //seedRenderer.material.SetColor("_Color", fadingCOlor);
            seedRenderer.material.SetColor("_EmissionColor", fadingCOlor);
            yield return null;
        }
        yield return null;
    }
}
