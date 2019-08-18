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

    public void PlayTTS(string _speech)
    {
        StartCoroutine(SpeechDownloader.DownloadTheAudio(audioSource, _speech));
    }
}
