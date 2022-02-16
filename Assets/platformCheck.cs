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

    public static bool CalibrationActivated = false;

    public Transform lineStructure;
    public Transform dottedStructure;



    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 30;

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

        int calibrationMode = PlayerPrefs.GetInt("calibrationMode");
        //calibrationMode = 1;
        if (calibrationMode != 0)
        {
            CalibrationActivated = true;
        }


        if (PlayerPrefs.GetInt("hasStructure") == 1)
        {
            //prepareProgrmmateurMode();
            prepareProgrmmateurMode(lineStructure);
            prepareProgrmmateurMode(dottedStructure);
        }

    }

    void prepareProgrmmateurMode(Transform st)
    {
        for (int i = 0; i < st.transform.GetChildCount(); i++)
        {
            Transform stPiece = st.transform.GetChild(i);
            stPiece.gameObject.SetActive(true);
            stPiece.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Input.touchCount == 2) || (Input.GetKeyDown(KeyCode.D)))
        {
            
            calibrationButton.SetActive(true);
        }
    }

    public void LoadCalibrationScene()
    {
        calibrationButton.GetComponent<Button>().interactable = false;
        CalibrationActivated = true;
        //SceneManager.LoadScene("calibration_tool");
    }

}
