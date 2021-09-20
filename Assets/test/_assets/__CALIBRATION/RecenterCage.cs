using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;


public class RecenterCage : MonoBehaviour
{
    public ARTrackedImageManager m_trackedImageManager;
    public Transform cageRoot;
    // Start is called before the first frame update

    void Update()
    {
        if (Input.touchCount == 3)
            recenterIt();
                
    }

    public void recenterIt()
    {
        m_trackedImageManager.enabled = true;

    }

    IEnumerator __kickRencenter()
    {
        yield return new WaitForSeconds(1.0f);
        m_trackedImageManager.trackedImagesChanged += ImageChanged;

        yield return null;
    }
    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        if(trackedImage.name == "Spot2")
        {
            Vector3 newPos = trackedImage.transform.position + (trackedImage.transform.forward * 3.5f);
            //Vector3 newPos = _spot2.position + (_spot2.forward * 2.5f);

            Vector3 lookAtPo = trackedImage.transform.position + (trackedImage.transform.position * 10.0f);
            setCagePosition(newPos, lookAtPo);
            m_trackedImageManager.trackedImagesChanged -= ImageChanged;
            m_trackedImageManager.enabled = false;
        }
    }

    public void setCagePosition(Vector3 pos, Vector3 lookAtPos)
    //public void setCagePosition(Vector3 pos, Quaternion rot)
    {
        GameObject lienRoot = GameObject.Find("LINE_Root");
        GameObject dottedRoot = GameObject.Find("DOTTED_Root");

        cageRoot.position = pos;
        //gameObject.transform.rotation = rot;
        cageRoot.LookAt(lookAtPos);
        cageState.spawnCages(dottedRoot.transform);
        cageState.spawnCages(lienRoot.transform);
        //gameObject.AddComponent<ARAnchor>();
    }

}
