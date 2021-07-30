using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

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


    [MenuItem("SO / Update Linerenderer")]
    static void AddLineRenderer()
    {
        string cageMat = "Assets/test/_assets/CONNECTION/cageMat.mat";
        string vfxAsset = "Assets/test/_assets/CONNECTION/src/connectionFX.vfx";
        Material SRm = (Material)AssetDatabase.LoadAssetAtPath(cageMat, typeof(Material));


        GameObject currGo = Selection.gameObjects[0];

        cageState.spawnCages(currGo.transform);
       /* if ((currGo.name == "lineCAGE") || (currGo.name == "dottedCAGE"))
        {
            Transform structure2 = currGo.transform.GetChild(0);
            //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
            for(int j =0; j< structure2.childCount; j++)
            {
                for (int i = 0; i < structure2.GetChild(j).childCount; i++)
                {
                    Transform ln = structure2.GetChild(j).GetChild(i);
                    if (!ln.name.StartsWith("cv_"))
                        continue;
                    LineRenderer lr = null;
                    lr = ln.GetComponent<LineRenderer>();
                    if (!lr)
                        lr = ln.gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
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
            }
        }*/
        /*else
        {
            //Material connMat = currGo.GetComponent<gpConnector>().connectorMat;
            for (int i = 0; i < currGo.transform.childCount; i++)
            {
                Transform ln = currGo.transform.GetChild(i);
                VisualEffect vfx = ln.gameObject.AddComponent<VisualEffect>();
                if (!vfx)
                    continue;
                vfx.visualEffectAsset = (VisualEffectAsset)AssetDatabase.LoadAssetAtPath(vfxAsset, typeof(VisualEffectAsset));
                Transform pt1 = ln.GetChild(0);
                Transform pt2 = ln.GetChild(1);
                vfx.SetVector3("startPoint", pt1.position);
                vfx.SetVector3("endPoint", pt2.position);
            }
        }*/

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
    }

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


#endif
}
