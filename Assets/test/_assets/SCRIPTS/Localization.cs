using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Localization : MonoBehaviour
{
    [System.Serializable]
    public class enImage
    {
        [SerializeField]
        public Image uiImage;

        [SerializeField]
        public Sprite vEn;
    }

    [System.Serializable]
    public class enAudio
    {
        [SerializeField]
        public AudioSource uiAudio;

        [SerializeField]
        public AudioClip vEn;
    }




    [SerializeField]
    public List<enImage> uiImages;

    [SerializeField]
    public List<enAudio> enAudios;

    void Start()
    {
        foreach (enImage enImg in uiImages)
        {
            enImg.uiImage.sprite = enImg.vEn;
        }
        foreach (enAudio enAud in enAudios)
        {
            enAud.uiAudio.clip = enAud.vEn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
