using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class windHotSpot : MonoBehaviour
{

    //[SerializeField]
    private VisualEffect visualEffect;
    public BoxCollider spawnSpot;
    // Start is called before the first frame update


    void Start()
    {
        visualEffect = gameObject.GetComponent<VisualEffect>();
        StartCoroutine("__kick");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator __kick()
    {

        for (; ; )
        {
            yield return new WaitForSeconds(Random.Range(2, 4.0f));
            Vector3 min = spawnSpot.bounds.min;
            Vector3 max = spawnSpot.bounds.max;

            Vector3 vecPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            Vector3 vecFieldPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));

            /*visualEffect.SetFloat("Intensity", Random.Range(5.0f, 15.0f));
            visualEffect.SetFloat("Drag", Random.Range(5.0f, 20.0f));
            visualEffect.SetFloat("Rotation", Random.Range(5.0f, 360.0f));*/

            visualEffect.SetVector3("vecPosition", vecPos);
            visualEffect.SetVector3("vecFieldCenter", vecFieldPos);
            

        }

    }
}
   