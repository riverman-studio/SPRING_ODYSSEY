using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
//[RequireComponent(typeof(ARAnchorManager))]
//[RequireComponent(typeof(ARRaycastManager))]

public class ImageTracking : MonoBehaviour
{
    public Animator masterAnimator;
    public Animator uiAnimator;
    [SerializeField]
    public GameObject placablePrefabs;
    private GameObject spawnedPrefabs = null;

    public cageState cageCtrl;

    [SerializeField]
    public GameObject imageDetectorPrefab;
    private GameObject spawnedDetectorPrefabs = null;

    private ARTrackedImageManager m_trackedImageManager;

    private bool imageDetected = false;
    ARTrackedImage _currentlyTracking = null;

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
        bool bTouching = (Input.touches.Length > 0) || (Input.GetMouseButton(0));
        bool TrigPoint1 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("01_ARTrigger01");
        bool TrigPoint2 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
        bool inUITargetLoop = uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("TargetLoop");
        bool inUITargetFill = uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("TargetFill");
        bool inUITargetFadeOut = uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("TargetFadeOut");

        if ((TrigPoint1 || TrigPoint2) && (inUITargetLoop || inUITargetFill))
        {
            m_trackedImageManager.enabled = true;
        }
        else
        {
            m_trackedImageManager.enabled = false;
            uiAnimator.SetBool("is_detected", false);
        }
            




        if (m_trackedImageManager.enabled)
        {
            if(_currentlyTracking)
            {
                if (_currentlyTracking.trackingState != TrackingState.Tracking)
                {
                    spawnedDetectorPrefabs.transform.position.Set(0.0f, 1000.0f, 0.0f);
                    spawnedDetectorPrefabs.name = "";
                    imageDetected = false;
                    uiAnimator.SetBool("is_detected", false);
                }
            }
        }
        if (inUITargetFadeOut)
        {
            spawnedDetectorPrefabs.transform.position = _currentlyTracking.transform.position;
            spawnedDetectorPrefabs.transform.rotation = _currentlyTracking.transform.rotation;
            spawnedDetectorPrefabs.name = _currentlyTracking.referenceImage.name;
            //if (imageDetected)

            //spawnAnchor2(spawnedPrefabs);
            cageCtrl.SetImageSpot(spawnedDetectorPrefabs.transform);
            imageDetected = false;
            _currentlyTracking = null;
            spawnedDetectorPrefabs.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            spawnedDetectorPrefabs.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            if (TrigPoint1)
            {
                spawnedDetectorPrefabs.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                StartCoroutine(__hideMarker(spawnedDetectorPrefabs.transform.GetChild(0).GetChild(0).gameObject));
                
            }
            if (TrigPoint2)
            {
                spawnedDetectorPrefabs.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                StartCoroutine(__hideMarker(spawnedDetectorPrefabs.transform.GetChild(0).GetChild(1).gameObject));
            }
        }
    }

    IEnumerator __hideMarker(GameObject aiMarker)
    {
        if(aiMarker.activeSelf)
        {
            yield return new WaitForSeconds(3.0f);
            aiMarker.SetActive(false);
        }
        yield return null;
    }
    
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        bool TrigPoint1 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("01_ARTrigger01");
        bool TrigPoint2 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
        bool go = false;
        if (TrigPoint1 || TrigPoint2)
            go = true;

        _currentlyTracking = trackedImage;
        //spawnedDetectorPrefabs.SetActive(true);
        imageDetected = false;
        if (go)
        {
            imageDetected = true;
            uiAnimator.SetBool("is_detected", true);
        }
    }
    


}

