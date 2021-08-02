using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
[RequireComponent(typeof(ARTrackedImageManager))]
[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]

public class ImageTracking : MonoBehaviour
{
    public Animator masterAnimator;
    [SerializeField]
    public GameObject placablePrefabs;
    private GameObject spawnedPrefabs = null;

    public cageState cageCtrl;

    [SerializeField]
    public GameObject imageDetectorPrefab;
    private GameObject spawnedDetectorPrefabs = null;

    private ARTrackedImageManager m_trackedImageManager;




    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    List<ARAnchor> m_Anchors = new List<ARAnchor>();
    List<TrackableId> m_anchorIds = new List<TrackableId>();

    public GameObject prefab
    {
        get => placablePrefabs;
        set => placablePrefabs = value;
    }


    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();

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
    private bool added = false;
    private bool imageDetected = false;
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
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //spawnedDetectorPrefabs.SetActive(false);
            imageDetected = false;
        }
    }
    private void Update()
    {
        bool bTouching = (Input.touches.Length > 0) || (Input.GetMouseButton(0));
        if (imageDetected &&  bTouching)
        {
            //spawnAnchor2(spawnedPrefabs);
            cageCtrl.SetImageSpot(spawnedDetectorPrefabs.transform);
        }
        if(cageCtrl.isCageActivated())
        {
            m_trackedImageManager.enabled = false;
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

        //spawnedDetectorPrefabs.SetActive(true);
        imageDetected = false;
        if(go)
        {
            spawnedDetectorPrefabs.transform.position = trackedImage.transform.position;
            spawnedDetectorPrefabs.transform.rotation = trackedImage.transform.rotation;
            spawnedDetectorPrefabs.name = trackedImage.referenceImage.name;
            imageDetected = true;
        }
        else
        {
            spawnedDetectorPrefabs.transform.position.Set(0.0f, 1000.0f, 0.0f);
        }

        
    }
    public void spawnAnchor2(GameObject trackedImage)
    {
        bool bTouching = (Input.touches.Length > 0) || (Input.GetMouseButton(0));
        if (!bTouching)
            return;
        if (added)
            return;

        Debug.Log("sending ray");
        Transform cameraTransform = Camera.main.transform;

        // Raycast against planes and feature points
        const TrackableType trackableTypes =
            TrackableType.FeaturePoint |
            TrackableType.PlaneWithinPolygon;

        Vector3 vDir = (trackedImage.transform.position - cameraTransform.position).normalized;
        Ray ray = new Ray(cameraTransform.position, vDir);

        // Perform the raycast
        if (m_RaycastManager.Raycast(ray, s_Hits, trackableTypes))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            ARRaycastHit hit = s_Hits[0];

            // Create a new anchor
            Pose anchorPose = new Pose(trackedImage.transform.position, hit.pose.rotation);
            var anchor = CreateAnchor(hit, anchorPose);
            added = true;

            //delete all anchors
            foreach (ARAnchor oldanchor in m_Anchors)
            {
                Destroy(oldanchor.gameObject);
            }
            m_Anchors.Clear();



            if (!anchor)
            {
                // Remember the anchor so we can remove it later.
                m_Anchors.Add(anchor);
                m_anchorIds.Add(anchor.trackableId);
            }
            else
            {
                Debug.Log("Error creating anchor");
            }
        }
        else
        {
            Debug.Log("Ray cast not hitting");
        }
    }

    public void spawnAnchor(ARTrackedImage trackedImage)
    {
        bool bTouching = (Input.touches.Length > 0) || (Input.GetMouseButton(0));
        if (!bTouching)
            return;
        if (added)
            return;

        Debug.Log("sending ray");
        Transform cameraTransform = Camera.main.transform;

        // Raycast against planes and feature points
        const TrackableType trackableTypes =
            TrackableType.FeaturePoint |
            TrackableType.PlaneWithinPolygon;

        Vector3 vDir = (trackedImage.transform.position - cameraTransform.position).normalized;
        Ray ray = new Ray(cameraTransform.position, vDir);

        // Perform the raycast
        if (m_RaycastManager.Raycast(ray, s_Hits, trackableTypes))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            ARRaycastHit hit = s_Hits[0];

            // Create a new anchor
            Pose anchorPose = new Pose(trackedImage.transform.position, hit.pose.rotation);
            var anchor = CreateAnchor(hit, anchorPose);
            added = true;

            //delete all anchors
            foreach (ARAnchor oldanchor in m_Anchors)
            {
                Destroy(oldanchor.gameObject);
            }
            m_Anchors.Clear();



            if (!anchor)
            {
                // Remember the anchor so we can remove it later.
                m_Anchors.Add(anchor);
                m_anchorIds.Add(anchor.trackableId);
            }
            else
            {
                Debug.Log("Error creating anchor");
            }
        }
        else
        {
            Debug.Log("Ray cast not hitting");
        }
    }
    // Start is called before the first frame update
    ARAnchor CreateAnchor(in ARRaycastHit hit, Pose anchorPose)
    {
        ARAnchor anchor = null;



        // If we hit a plane, try to "attach" the anchor to the plane
        if (hit.trackable is ARPlane plane)
        {
            var planeManager = GetComponent<ARPlaneManager>();
            if (planeManager)
            {
                Debug.Log("Creating anchor attachment.");
                var oldPrefab = m_AnchorManager.anchorPrefab;
                m_AnchorManager.anchorPrefab = prefab;
                anchor = m_AnchorManager.AttachAnchor(plane, anchorPose);
                m_AnchorManager.anchorPrefab = oldPrefab;
                return anchor;
            }
        }
        else
        {
            Debug.Log("ARPlane not hit");
        }

        // Otherwise, just create a regular anchor at the hit pose
        Debug.Log("Creating regular anchor.");

        // Note: the anchor can be anywhere in the scene hierarchy
        var gameObject = Instantiate(prefab, anchorPose.position, anchorPose.rotation);

        // Make sure the new GameObject has an ARAnchor component
        anchor = gameObject.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = gameObject.AddComponent<ARAnchor>();
        }


        return anchor;
    }
}

