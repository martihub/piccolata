using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtras : MonoBehaviour
{

    public static PlayerExtras instance;
    public AudioSource audioSource;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
    }


    public void PlayTTS(string speech)
    {
        StartCoroutine(DownloadTheAudio(speech));
    }

    IEnumerator DownloadTheAudio(string speech)
    {
        string url;
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + speech + "&tl=En-gb";
        WWW www = new WWW(url);
        yield return www;
        audioSource.clip = www.GetAudioClip(false, false, AudioType.MPEG);
        audioSource.Play();
        Debug.Log("GELDİ");
    }


}
