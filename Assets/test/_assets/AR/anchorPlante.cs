using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class anchorPlante : MonoBehaviour
{
    [SerializeField]
    GameObject m_Prefab;

    [SerializeField]
    ARSession m_ARSession;

    public Text txt;

    bool bSavedState = false;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    List<ARAnchor> m_Anchors = new List<ARAnchor>();
    List<TrackableId> m_anchorIds = new List<TrackableId>();

    public GameObject prefab
    {
        get => m_Prefab;
        set => m_Prefab = value;
    }

    ARRaycastManager m_RaycastManager;
    ARAnchorManager m_AnchorManager;

    // Start is called before the first frame update
    ARAnchor CreateAnchor(in ARRaycastHit hit)
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
                anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                m_AnchorManager.anchorPrefab = oldPrefab;
                return anchor;
            }
        }

        // Otherwise, just create a regular anchor at the hit pose
        Debug.Log("Creating regular anchor.");

        // Note: the anchor can be anywhere in the scene hierarchy
        var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);

        // Make sure the new GameObject has an ARAnchor component
        anchor = gameObject.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = gameObject.AddComponent<ARAnchor>();
        }


        return anchor;
    }

    public void spawnAnchor()
    {
        // Raycast against planes and feature points
        const TrackableType trackableTypes =
            TrackableType.FeaturePoint |
            TrackableType.PlaneWithinPolygon;

        // Perform the raycast
        if (m_RaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), s_Hits, trackableTypes))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            var hit = s_Hits[0];

            // Create a new anchor
            var anchor = CreateAnchor(hit);

            //delete all anchors
            foreach (ARAnchor oldanchor in m_Anchors)
            {
                Destroy(oldanchor.gameObject);
            }
            m_Anchors.Clear();



            if (anchor)
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
    }

    void Update()
    {
    }

    public void resetState()
    {
        bSavedState = true;

    }

    public void restoreState()
    {
#if UNITY_IOS

        StartCoroutine("__restoreAnchor");
#endif

    }

    IEnumerator __restoreAnchor()
    {
#if UNITY_IOS
        var sessionSubsystem = (ARKitSessionSubsystem)m_ARSession.subsystem;
        ARWorldMapRequest request = sessionSubsystem.GetARWorldMapAsync();
        while (!request.status.IsDone())
            yield return null;
        if (request.status.IsError())
        {
            Debug.Log(string.Format("Session serialization failed with status {0}", request.status));
            yield break;
        }
        ARWorldMap worldMap = request.GetWorldMap();
        request.Dispose();

        foreach (TrackableId anchorId in m_anchorIds)
        {
            ARAnchor anchor =  m_AnchorManager.GetAnchor(anchorId);
            txt.text = anchor.GetInstanceID().ToString();

        }
        
#endif
        yield return null;
    }



    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
    }


}
