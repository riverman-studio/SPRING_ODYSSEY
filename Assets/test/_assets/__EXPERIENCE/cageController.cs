using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class cageController : MonoBehaviour
{

    public GameObject dottedCage;
    public GameObject linedCage;
    public GameObject plant;

    public bool dottedActivated = false;

    private Pose _spot1;
    private Pose _spot2;

    private bool _gotSpot1 = false;
    private bool _gotSpot2 = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameObject helperPCNode = GameObject.Find("__HELPERS");
            helperPCNode.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit HitInfo;
        Transform cameraTransform = Camera.main.transform;
        bool bTouching = false;
        Vector3 touchPos = new Vector3();
        //if(_state != ConnectionState.Reaching)
        if (Input.touches.Length > 0)
        {
            bTouching = true;
            touchPos = Input.touches[0].position;
        }
        else if (Input.GetMouseButton(0))
        {
            bTouching = true;
            touchPos = Input.mousePosition;
            touchPos.x = Screen.width / 2;
            touchPos.y = Screen.height / 2;

        }
        if (!bTouching)
            return;

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            //if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out HitInfo, 100.0f) && bTouching)
            if (Physics.Raycast(ray, out HitInfo, 100.0f))
            {
                if (HitInfo.collider.gameObject.name == "Spot1")
                {
                    _spot1.position = HitInfo.collider.gameObject.transform.position;
                    _spot1.rotation = HitInfo.collider.gameObject.transform.rotation;
                    _gotSpot1 = true;
                }
                if (HitInfo.collider.gameObject.name == "Spot2")
                {
                    _spot2.position = HitInfo.collider.gameObject.transform.position;
                    _spot2.rotation = HitInfo.collider.gameObject.transform.rotation;
                    _gotSpot2 = true;
                }

                if (_gotSpot1 && _gotSpot2 && !dottedActivated)
                {
                    Vector3 newPos = _spot1.position + ((_spot2.position - _spot1.position) * 2.0f);
                    ActivateDotted(newPos, Quaternion.identity);
                }
            }
        }

    }

    
    public void SetImageSpot(Transform txSport)
    {
        if (txSport.name == "Spot1")
        {
            _spot1.position = txSport.position;
            _spot1.rotation = txSport.rotation;
            _gotSpot1 = true;
        }
        if (txSport.name == "Spot2")
        {
            _spot2.position = txSport.position;
            _spot2.rotation = txSport.rotation;
            _gotSpot2 = true;
        }
        if (_gotSpot1 && _gotSpot2 && !dottedActivated)
        {
            Vector3 newPos = _spot1.position + ((_spot2.position - _spot1.position) * 4.0f);
            ActivateDotted(newPos, Quaternion.identity);
        }
    }

    public void ActivateDotted(Vector3 pos, Quaternion quat)
    {
        dottedActivated = true;
        dottedCage.transform.position = pos;
        dottedCage.transform.rotation = quat;

        cageController.spawnCages(dottedCage.transform);
        dottedCage.SetActive(true);
        setColorOfCage(dottedCage, 0.0f);
        FadeInCage(dottedCage);
    }


    public void ActivateLined()
    {
        linedCage.transform.position = dottedCage.transform.position;
        linedCage.transform.rotation = dottedCage.transform.rotation;

        cageController.spawnCages(linedCage.transform);
        linedCage.SetActive(true);

        /*setColorOfCage(linedCage, 0.0f);
        FadeInCage(linedCage);*/
    }


    void setColorOfCage(GameObject cage, float fAlpha)
    {
        Transform mainGo = cage.transform.GetChild(0);
        mainGo = mainGo.transform.GetChild(0);
        for (int j = 0; j < mainGo.transform.childCount; j++)
        {
            Transform levelChild = mainGo.transform.GetChild(j);
            for (int i = 0; i < levelChild.childCount; i++)
            {
                Transform cageSegment = levelChild.GetChild(i);
                VisualEffect vfx = cageSegment.GetComponent<VisualEffect>();
                if (vfx)
                {
                    //mb.fillTheLine((float)j);
                    vfx.SetFloat("alpha", fAlpha);
                }
                LineRenderer lr = cageSegment.GetComponent<LineRenderer>();
                if (lr)
                {
                    Color strandColor = lr.material.color;
                    strandColor.a = fAlpha;
                    lr.material.color = strandColor;
                    lr.startColor = strandColor;
                    lr.endColor = strandColor;
                    lr.SetColors(strandColor, strandColor);

                }
            }
        }
    }


    public static void spawnCages(Transform cageRoot)
    {
        GameObject currGo = cageRoot.gameObject;
        if ((currGo.name == "lineCAGE") || (currGo.name == "dottedCAGE"))
        {
            Transform structure2 = currGo.transform.GetChild(0);
            for (int j = 0; j < structure2.childCount; j++)
            {
                for (int i = 0; i < structure2.GetChild(j).childCount; i++)
                {
                    Transform ln = structure2.GetChild(j).GetChild(i);
                    if (!ln.name.StartsWith("cv_"))
                        continue;
                    LineRenderer lr = null;
                    lr = ln.GetComponent<LineRenderer>();
                    /*if (!lr)
                        lr = ln.gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;*/
                    Transform pt1 = ln.GetChild(0);
                    Transform pt2 = ln.GetChild(1);
                    Vector3[] positions = new Vector3[2];
                    positions[0] = new Vector3(0, 0, 0);
                    positions[0] = pt1.position;
                    positions[1] = new Vector3(0, 0, 0);
                    positions[1] = pt2.position;
                    lr.positionCount = positions.Length;
                    lr.SetPositions(positions);
                    lr.startWidth = 0.01f;
                    lr.textureMode = LineTextureMode.Tile;
                }
            }
        }
    }

    void FadeInCage(GameObject cage)
    {
        StartCoroutine(__fadeInCage(cage));
    }
    IEnumerator __fadeInCage(GameObject cage)
    {
        //yield return new WaitForSeconds(2.0f);
        float fAlpha = 0.0f;
        while (fAlpha < 1.0f)
        {
            fAlpha += (Time.deltaTime * 0.1f);
            setColorOfCage(cage, Mathf.Pow( fAlpha, 4) );
            yield return null;
        }
        setColorOfCage(dottedCage, 1.0f);
        yield return new WaitForSeconds(1.0f);
        FadeOutCage(cage);
        yield return null;
    }

    void FadeOutCage(GameObject cage)
    {
        StartCoroutine(__fadeOutCage(cage));
    }
    IEnumerator __fadeOutCage(GameObject cage)
    {
        float fAlpha = 1.0f;
        while (fAlpha > 0.0f)
        {
            fAlpha -= (Time.deltaTime * 0.1f);
            setColorOfCage(cage, Mathf.Pow(fAlpha, 4));
            yield return null;
        }
        setColorOfCage(dottedCage, 0.0f);
        if(!linedCage.activeSelf)
        {
            dottedCage.SetActive(false);
            plant.transform.position = dottedCage.transform.position;
            plant.transform.rotation = dottedCage.transform.rotation;
            plant.SetActive(true);
            ActivateLined();
        }

        yield return null;
    }

}