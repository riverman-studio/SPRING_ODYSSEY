using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class connectionInteraction : MonoBehaviour
{
    public Transform pt1;
    public Transform pt2;

    private VisualEffect visualEffect;

    // Start is called before the first frame update
    void Start()
    {
        visualEffect = gameObject.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
