using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformCheck : MonoBehaviour
{
    public GameObject fpsController;
    public GameObject arController;

    // Start is called before the first frame update
    void Awake()
    {
        fpsController.SetActive(true);
        arController.SetActive(false);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fpsController.SetActive(false);
            arController.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
