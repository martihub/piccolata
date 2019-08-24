using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGoogleWords : MonoBehaviour
{

    AudioSource audioSource;

    public static ManageGoogleWords instance;
    void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private string words;
    public void SetWords(string str)
    {
        words = str;
        StartCoroutine(SpeechDownloader.DownloadTheAudio(audioSource, words));
        Debug.Log(words);
    }

}
