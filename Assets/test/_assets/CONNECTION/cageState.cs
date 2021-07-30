using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class cageState : MonoBehaviour
{
    public Animator masterAnimator;

    public GameObject dottedCage;
    public float dottedCageAlpha = 0.0f;

    public GameObject lineCage;
    public float lineCageAlpha = 0.0f;

    // Start is called before the first frame update
    private Pose _spot1;
    private Pose _spot2;

    private bool _gotSpot1 = false;
    private bool _gotSpot2 = false;
    private bool dottedActivated = false;
    void Start()
    {
        
    }

    public bool isCageActivated()
    {
        return dottedActivated;
    }
    // Update is called once per frame
    void Update()
    {
        if(masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("06_StructureApparition"))
        {
            if(_gotSpot1 && _gotSpot2)
            {
                if (!dottedActivated)
                {
                    Vector3 newPos = _spot1.position + ((_spot2.position - _spot1.position) * 2.0f);
                    setCagePosition(newPos, Quaternion.identity);
                }
                setColorOfCage(dottedCage, dottedCageAlpha);
                //setColorOfCage(lineCage, dottedCageAlpha);
            }
        }
        if (masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("InteractionLigneToComplete"))
        {
            
        }
        bool TrigPoint1 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("01_ARTrigger01");
        bool TrigPoint2 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
        if (TrigPoint1 || TrigPoint2)
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
                    if ((HitInfo.collider.gameObject.name == "Spot1") || (TrigPoint1))
                    {
                        _spot1.position = HitInfo.collider.gameObject.transform.position;
                        _spot1.rotation = HitInfo.collider.gameObject.transform.rotation;
                        _gotSpot1 = true;
                        masterAnimator.SetTrigger("DebugNext");
                    }
                    if ((HitInfo.collider.gameObject.name == "Spot2")|| (TrigPoint2))
                    {
                        _spot2.position = HitInfo.collider.gameObject.transform.position;
                        _spot2.rotation = HitInfo.collider.gameObject.transform.rotation;
                        _gotSpot2 = true;
                        masterAnimator.SetTrigger("Next");
                    }


                }
            }
        }
    }
    public void SetImageSpot(Transform txSport)
    {
        bool TrigPoint1 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
        bool TrigPoint2 = masterAnimator.GetCurrentAnimatorStateInfo(0).IsName("05_ARTrigger02 1");
       
         if ((txSport.name == "Spot1")|| (TrigPoint1))
        {
            _spot1.position = txSport.position;
            _spot1.rotation = txSport.rotation;
            _gotSpot1 = true;
            masterAnimator.SetTrigger("DebugNext");
        }
        if ((txSport.name == "Spot2")|| (TrigPoint2))
        {
            _spot2.position = txSport.position;
            _spot2.rotation = txSport.rotation;
            _gotSpot2 = true;
            masterAnimator.SetTrigger("Next");
        }
       /* if (_gotSpot1 && _gotSpot2 && !dottedActivated)
        {
            Vector3 newPos = _spot1.position + ((_spot2.position - _spot1.position) * 4.0f);
            setCagePosition(newPos, Quaternion.identity);
        }*/
    }

    public void setCagePosition(Vector3 pos, Quaternion quat)
    {
        lineCage.SetActive(true);
        dottedCage.SetActive(true);
        dottedActivated = true;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = quat;
        cageState.spawnCages(dottedCage.transform);
        cageState.spawnCages(lineCage.transform);
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
        GameObject currGo = cageRoot.GetChild(0).gameObject;
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
#if UNITY_EDITOR
                    /*if (!lr)
                        lr = ln.gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;*/
#endif
                    if (!lr)
                        continue;
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
}
