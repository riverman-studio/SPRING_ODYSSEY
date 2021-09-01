using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.Animations;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class gpConnector : MonoBehaviour
{
    public Transform connHiLite;
    public Transform collisionCheck;
    //public Image crosshair;





    // Start is called before the first frame update
    public void activateEverySegment()
    {
        for (int j=0; j<gameObject.transform.childCount; j++)
        {
            Transform levelChild = gameObject.transform.GetChild(j);
            for ( int i=0; i<levelChild.childCount; i++)
            {
                Transform cageSegment = levelChild.GetChild(i);
                microBehaviour mb = cageSegment.GetComponent<microBehaviour>();
                if (mb)
                {
                    mb.fillTheLine((float)j);
                }
            }
        }
    }


    public void deActivateEverySegment()
    {
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Transform levelChild = gameObject.transform.GetChild(j);
            for (int i = 0; i < levelChild.childCount; i++)
            {
                Transform cageSegment = levelChild.GetChild(i);
                templateBehaviour mb = cageSegment.GetComponent<microBehaviour>() as templateBehaviour;
                if(!mb)
                    mb = cageSegment.GetComponent<sisterNodeBehaviour>() as templateBehaviour;
                if (!mb)
                    mb = cageSegment.GetComponent<nodeBehaviour>() as nodeBehaviour;
                if (mb)
                {
                    mb.unFillTheLine((float)(2-j));
                }
            }
        }
    }



    // Update is called once per frame
    public void crosshairHide()
    {
        //crosshair.gameObject.SetActive(false);
        //crosshair.color = GlobalVariables.colNone;
    }
    public void crosshairRed()
    {
        //crosshair.gameObject.SetActive(true);
        //crosshair.color = Color.red;
    }
    public void crosshairYellow()
    {
        //crosshair.gameObject.SetActive(true);
        //crosshair.color = Color.yellow;
    }

#if UNITY_EDITOR

    private static string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
    [MenuItem("SO / Move CAGE Root")]
    static void MoveCageRoot()
    {
        GameObject lienRoot = GameObject.Find("LINE_Root");
        GameObject dottedRoot = GameObject.Find("DOTTED_Root");
        cageState.spawnCages(lienRoot.transform);
        cageState.spawnCages(dottedRoot.transform);
    }
    [MenuItem("SO / Update constraint")]
    static void UpdateConstraint()
    {
        string cageMat = "Assets/test/_assets/CONNECTION/cageMat.mat";
        string vfxAsset = "Assets/test/_assets/CONNECTION/src/connectionFX.vfx";
        Material SRm = (Material)AssetDatabase.LoadAssetAtPath(cageMat, typeof(Material));

        


        GameObject lienRoot = GameObject.Find("LINE_Root"); 
        GameObject dottedRoot = GameObject.Find("DOTTED_Root");

        string lienRootPath = GetGameObjectPath(lienRoot.transform);
        string dottedRootPath = GetGameObjectPath(dottedRoot.transform);
        

        ParentConstraint[] parentContrs = lienRoot.GetComponentsInChildren<ParentConstraint>();

        foreach (ParentConstraint pc in parentContrs)
        {
            string lienGoPath = GetGameObjectPath(pc.gameObject.transform);

            
            string dottedGoPath = lienGoPath.Replace("LINE_Root", "DOTTED_Root");
            dottedGoPath = dottedGoPath.Replace("lineCAGE", "dottedCAGE");

            //Debug.Log(dottedGoPath);
            GameObject dottedNode = GameObject.Find(dottedGoPath);
            dottedNode.transform.localPosition = pc.gameObject.transform.localPosition;
        }


        cageState.spawnCages(lienRoot.transform);
        cageState.spawnCages(dottedRoot.transform);
    }

    /*
    [MenuItem("SO / remove Linerenderer")]
    static void RemoveLineRenderer()
    {
        GameObject currGo = Selection.gameObjects[0];
        //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
        for (int i = 0; i < currGo.transform.childCount; i++)
        {
            Transform ln = currGo.transform.GetChild(i);
            VisualEffect vfx = null;
            vfx = ln.GetComponent<VisualEffect>();
            if(vfx)
            {
                DestroyImmediate(vfx);
            }
            LineRenderer lr = ln.GetComponent<LineRenderer>();
            if(lr)
            {
                DestroyImmediate(lr);
            }
        }
    }*/
    [MenuItem("SO / copyTransforms")]
    static void copyTransforms()
    {
        GameObject lienRoot = GameObject.Find("lineCAGE");
        GameObject dottedRoot = GameObject.Find("dottedCAGE");

        Transform lineStructure2 = lienRoot.transform.GetChild(0);
        Transform dottedStructure2 = dottedRoot.transform.GetChild(0);

        for (int i=0; i< lineStructure2.childCount; i++)
        {
            Transform lsLV = lineStructure2.GetChild(i);
            for (int j = 0; j < lsLV.childCount; j++)
            {
                Transform lscv = lsLV.GetChild(j);
                string lscvPath = GetGameObjectPath(lscv);
                string dscvPath = lscvPath.Replace("LINE_Root", "DOTTED_Root");
                dscvPath = dscvPath.Replace("lineCAGE", "dottedCAGE");
                if (lscvPath.Contains("cv_L2_18"))
                {
                    int brk = 0;

                }
                GameObject _go = GameObject.Find(dscvPath);
                if (_go == null)
                    continue;
                try
                {
                    Transform dscv = _go.transform;
                    dscv.localPosition = lscv.localPosition;
                    dscv.GetChild(0).localPosition = lscv.GetChild(0).localPosition;
                    dscv.GetChild(1).localPosition = lscv.GetChild(1).localPosition;
                }
                catch (Exception e)
                {
                    Debug.Log(dscvPath);
                }


            }
        }

    }
    /*
    [MenuItem("SO / set line render width")]
    static void SetLineRendererWidth()
    {
        GameObject currGo = Selection.gameObjects[0];
        //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
        for (int i = 0; i < currGo.transform.childCount; i++)
        {
            Transform ln = currGo.transform.GetChild(i);
            LineRenderer lr = null;
            lr = ln.GetComponent<LineRenderer>();
            if (lr)
            {
                lr.startWidth = 0.01f;
            }
        }
    }

    [MenuItem("SO / add microBehaviour")]
    static void addMicroBehaviour()
    {
        GameObject currGo = Selection.gameObjects[0];
        //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
        for (int i = 0; i < currGo.transform.childCount; i++)
        {
            Transform ln = currGo.transform.GetChild(i);
            GameObject goSegment = ln.gameObject;
            if (goSegment.GetComponent<nodeBehaviour>() != null)
                continue;
            if (goSegment.GetComponent<sisterNodeBehaviour>() != null)
                continue;

            ln.gameObject.AddComponent<microBehaviour>();
            LineRenderer lr = null;
            lr = ln.GetComponent<LineRenderer>();
            if (lr == null)
                continue;
            lr.material.color = Color.white;
            lr.startColor = Color.white;
            lr.endColor = Color.white;
            lr.SetColors(Color.white, Color.white);

        }
    }/*

    [MenuItem("SO / correctCageBars")]
    static void correctCageBars()
    {
        string cageMat = "Assets/test/_assets/CONNECTION/cageMat.mat";
        GameObject currGo = Selection.gameObjects[0];
        string vfxAsset = "Assets/test/_assets/CONNECTION/src/connectionFX.vfx";
        Material SRm = (Material)AssetDatabase.LoadAssetAtPath(cageMat, typeof(Material));
        //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
        for (int i = 0; i < currGo.transform.childCount; i++)
        {
            Transform ln = currGo.transform.GetChild(i);
            LineRenderer lr = ln.GetComponent<LineRenderer>();
            VisualEffect vfx = ln.GetComponent<VisualEffect>();

            if(lr)
            {
                Transform pt1 = ln.GetChild(0);
                Transform pt2 = ln.GetChild(1);
                Vector3[] positions = new Vector3[2];
                positions[0] = new Vector3(0, 0, 0);
                positions[0] = pt1.position;
                positions[1] = new Vector3(0, 0, 0);
                positions[1] = pt2.position;
                lr.positionCount = positions.Length;
                lr.SetPositions(positions);
                lr.material = SRm;
                lr.startWidth = 0.01f;
                lr.textureMode = LineTextureMode.Tile;
            }
            if(vfx)
            {
                vfx.visualEffectAsset = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath(vfxAsset, typeof(VisualEffectAsset));
                Transform pt1 = ln.GetChild(0);
                Transform pt2 = ln.GetChild(1);

                vfx.SetVector3("startPoint", pt1.position);
                vfx.SetVector3("endPoint", pt2.position);
            }
        }
    }
    */

#endif
}
