using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radiationSpawner : MonoBehaviour
{
    public GameObject unitRadiation;
    public bool boom = false;
    Animator _unitRadiationAnimator = null;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GameObject unitRadiationInstance = Instantiate(unitRadiation, transform.position, Quaternion.identity);
        _unitRadiationAnimator = unitRadiationInstance.GetComponent<Animator>();
        Transform radDir = transform.parent.GetChild(0);
        unitRadiationInstance.GetComponent<radiationBoum>().radiationDirection = radDir.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(boom)
        {
            if(_unitRadiationAnimator != null)
                _unitRadiationAnimator.SetTrigger("Boum");
        }
    }
}
