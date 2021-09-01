using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
//[RequireComponent(typeof(ARAnchorManager))]
//[RequireComponent(typeof(ARRaycastManager))]

public class CalibrationTool : MonoBehaviour
{
    [SerializeField]
    public GameObject placablePrefabs;
    private GameObject spawnedPrefabs = null;

    public cageState cageCtrl;
    public Slider scalingSlider;
    public Text scaleValue;

    [SerializeField]
    public GameObject imageDetectorPrefab;
    private GameObject spawnedDetectorPrefabs = null;

    private ARTrackedImageManager m_trackedImageManager;

    private bool imageDetected = false;

    public GameObject prefab
    {
        get => placablePrefabs;
        set => placablePrefabs = value;
    }


    private void Awake()
    {

        m_trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        spawnedPrefabs = Instantiate(placablePrefabs, new Vector3(0.0f, 200.0f, 0.0f), Quaternion.identity);
        spawnedDetectorPrefabs = Instantiate(imageDetectorPrefab, new Vector3(0.0f, 200.0f, 0.0f), Quaternion.identity);
    }

    private void OnEnable()
    {
        m_trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        m_trackedImageManager.trackedImagesChanged -= ImageChanged;
    }


    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
    }
  
    private void Update()
    {

        if (Input.touchCount == 5) 
        {
            cageCtrl.transform.position = spawnedDetectorPrefabs.transform.position;
            cageCtrl.transform.rotation = spawnedDetectorPrefabs.transform.rotation;
            StartCoroutine(__correctCage());
        }


        scaleValue.text = cageCtrl.gameObject.transform.localScale.x.ToString();
    }

    IEnumerator __correctCage()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject lienRoot = GameObject.Find("LINE_Root");
        GameObject dottedRoot = GameObject.Find("DOTTED_Root");
        cageState.spawnCages(lienRoot.transform);
        cageState.spawnCages(dottedRoot.transform);
        yield return null;
    }
    
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        spawnedDetectorPrefabs.transform.position = trackedImage.transform.position;
        spawnedDetectorPrefabs.transform.rotation = trackedImage.transform.rotation;
    }


    public void scaleValueChange()
    {
        //Debug.Log(scalingSlider.value);
        //   cageCtrl.transform.localScale.Set(scalingSlider.value, scalingSlider.value, scalingSlider.value);
        cageCtrl.gameObject.transform.localScale += new Vector3(scalingSlider.value, scalingSlider.value, scalingSlider.value);
    }

}

