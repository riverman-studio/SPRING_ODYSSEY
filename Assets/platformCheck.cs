using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class platformCheck : MonoBehaviour
{
    public GameObject inFront;
    public GameObject fpsController;
    public GameObject arController;
    public GameObject calibrationButton;

    private bool btnCalibrationActivated = false;

    // Start is called before the first frame update
    void Awake()
    {

        ParentConstraint parentConstr = inFront.GetComponent<ParentConstraint>();


        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fpsController.SetActive(false);
            arController.SetActive(true);
            GameObject helperPCNode = GameObject.Find("__HELPERS");
            helperPCNode.SetActive(false);

            ConstraintSource constraint = new ConstraintSource();
            constraint.sourceTransform = arController.transform.Find("AR Session/AR Session Origin/AR Camera/attachObject");
            if (!constraint.sourceTransform)
                constraint.sourceTransform = arController.transform.GetChild(1).GetChild(0).GetChild(0);
            constraint.weight = 1.0f;
            parentConstr.AddSource(constraint);

        }
        else 
        {
            fpsController.SetActive(true);
            arController.SetActive(false);

            ConstraintSource constraint = new ConstraintSource();
            constraint.sourceTransform = fpsController.transform.Find("Camera/attachObject");
            constraint.weight = 1.0f;
            parentConstr.AddSource(constraint);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Input.touchCount == 2) || (Input.GetKeyDown(KeyCode.D)))
        {
            btnCalibrationActivated = !btnCalibrationActivated;
            calibrationButton.SetActive(btnCalibrationActivated);
        }
    }

    public void LoadCalibrationScene()
    {
        SceneManager.LoadScene("calibration_tool");
    }

}
