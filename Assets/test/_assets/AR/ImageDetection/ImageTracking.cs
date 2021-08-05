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
    Coroutine _arTrackHelper = null;
    float fTrackerTiming = 0.0f;
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
        //if (imageDetected && bTouching)
        if (imageDetected)
        {
            //spawnAnchor2(spawnedPrefabs);
            cageCtrl.SetImageSpot(spawnedDetectorPrefabs.transform);
            imageDetected = false;
        }
        if (cageCtrl.isCageActivated())
        {
            m_trackedImageManager.enabled = false;
        }
        if(m_trackedImageManager.enabled)
        {
            if(_currentlyTracking)
            {
                if (_currentlyTracking.trackingState != TrackingState.Tracking)
                {
                    spawnedDetectorPrefabs.transform.position.Set(0.0f, 1000.0f, 0.0f);
                    imageDetected = false;
                    if (_arTrackHelper != null)
                    {
                        StopCoroutine(_arTrackHelper);
                        fTrackerTiming = 0.0f;
                        _arTrackHelper = null;
                        _currentlyTracking = null;
                        uiAnimator.SetFloat("Fill", fTrackerTiming);
                    }
                }
            }
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        bool TrigPoint1 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("01_ARTrigger01");
        bool TrigPoint2 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
        bool go = false;
        if ((TrigPoint1) && (trackedImage.referenceImage.name == "Spot1"))
            go = true;
        else if ((TrigPoint2) && (trackedImage.referenceImage.name == "Spot2"))
            go = true;
        _currentlyTracking = trackedImage;
        //spawnedDetectorPrefabs.SetActive(true);
        imageDetected = false;
        if (go)
        {
            if (_arTrackHelper == null)
            {
                _arTrackHelper = StartCoroutine(__timeArHelper());
            }
            if (_arTrackHelper != null)
            {
                if (fTrackerTiming > 2.0f)
                {
                    spawnedDetectorPrefabs.transform.position = trackedImage.transform.position;
                    spawnedDetectorPrefabs.transform.rotation = trackedImage.transform.rotation;
                    spawnedDetectorPrefabs.name = trackedImage.referenceImage.name;
                    imageDetected = true;
                    if (_arTrackHelper != null)
                    {
                        StopCoroutine(_arTrackHelper);
                        fTrackerTiming = 0.0f;
                        _arTrackHelper = null;
                    }
                }
            }

        }
    }
    
    IEnumerator __timeArHelper()
    {
        while(fTrackerTiming < 2.3f)
        {
            uiAnimator.SetFloat("Fill", fTrackerTiming);
            fTrackerTiming += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }


}

