using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Localization : MonoBehaviour
{
    public Text btnLanguageText;
    [System.Serializable]
    public class enImage
    {
        [SerializeField]
        public Image uiImage;

        [SerializeField]
        public Sprite vEn;

        private Sprite vFr;

        public void init()
        {
            vFr = uiImage.sprite;
        }
        public Sprite getVFr() { return vFr; }
        public void setVFr(Sprite v_Fr) { vFr = v_Fr; }
    }

    [System.Serializable]
    public class enAudio
    {
        [SerializeField]
        public AudioSource uiAudio;

        [SerializeField]
        public AudioClip vEn;

        //private AudioClip vFr;

        private AudioClip vFr;

        public AudioClip getVFr() { return vFr; }
        public void init()
        {
            vFr = uiAudio.clip;
        }
        public void setVFr(AudioClip v_Fr) { vFr = v_Fr; }
    }

    [System.Serializable]
    public class enText
    {
        [SerializeField]
        public Text uiText;

        [SerializeField]
        public string vEn;

        private string vFr;

        public void init()
        {
            vFr = uiText.text;
        }
        public string getVFr() { return vFr; }
        public void setVFr(string v_Fr) { vFr = v_Fr; }
    }


    [SerializeField]
    public List<enImage> uiImages;

    [SerializeField]
    public List<enAudio> enAudios;

    [SerializeField]
    public List<enText> enTextes;

    void Start()
    {
        foreach (enImage enImg in uiImages)
        {
            enImg.init();
        }
        foreach (enAudio enAud in enAudios)
        {
            enAud.init();
        }
        foreach (enText enTxt in enTextes)
        {
            enTxt.init();
        }
        //SwitchToEnglish();
        //PlayableDirector.RebuildGraph();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchLanguage()
    {
        if(btnLanguageText.text == "EN")
        {
            btnLanguageText.text = "FR";
            SwitchToEnglish();
        }
        else if (btnLanguageText.text == "FR")
        {
            btnLanguageText.text = "EN";
            SwitchToFrench();
        }
    }
    public void SwitchToFrench()
    {
        foreach (enImage enImg in uiImages)
        {
            enImg.uiImage.sprite = enImg.getVFr();
        }
        foreach (enText enTxt in enTextes)
        {
            enTxt.uiText.text = enTxt.getVFr();
        }
        foreach (enAudio enAud in enAudios)
        {
            enAud.uiAudio.Stop();
            enAud.uiAudio.clip = null;
            enAud.uiAudio.clip = enAud.vEn;
        }
    }

    public void SwitchToEnglish()
    {
        foreach (enImage enImg in uiImages)
        {
            enImg.uiImage.sprite = enImg.vEn;
        }
        foreach (enText enTxt in enTextes)
        {
            enTxt.uiText.text = enTxt.vEn;
        }
        foreach (enAudio enAud in enAudios)
        {
            enAud.uiAudio.Stop();
            enAud.uiAudio.clip = null;
            enAud.uiAudio.clip = enAud.vEn;
        }
    }

}
