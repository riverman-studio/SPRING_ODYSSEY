using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class preLandingPage : MonoBehaviour
{
    // Start is called before the first frame update
    static Color bntClickCol = new Color(0.93f, 0.64f, 0.0f);

    void Start()
    {
        PlayerPrefs.SetInt("calibrationMode", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadExpoVersion(Text btnText)
    {
        btnText.color = preLandingPage.bntClickCol;
        //SceneManager.LoadScene("main");
        StartCoroutine(__loadScene("main"));
        PlayerPrefs.SetInt("hasStructure", 0);
    }
    public void loadProgVersion(Text btnText)
    {
        btnText.color = preLandingPage.bntClickCol;
        //SceneManager.LoadScene("mainWithStructure");
        StartCoroutine(__loadScene("main"));
        PlayerPrefs.SetInt("hasStructure", 1);
    }

    public void toggleCalibrationMode()
    {
        int calibrationMode = PlayerPrefs.GetInt("calibrationMode");
        calibrationMode = 1 - calibrationMode;
        PlayerPrefs.SetInt("calibrationMode", calibrationMode);
    }

    IEnumerator __loadScene(string sceneName)
    {
        //yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

}
