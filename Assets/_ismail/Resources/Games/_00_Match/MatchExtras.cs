using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class MatchExtras : MonoBehaviour
{


    public GameObject player;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] successAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] failureAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public Word[] successWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public Word[] motivationWords;
    AudioSource audioSource;

    public static MatchExtras instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        successWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/SuccessWords.json");
        motivationWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/MotivationWords.json");
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccessAudio(int selected)
    {
        audioSource.PlayOneShot(successAudio[selected]);
    }

    public void PlayFailureAudio(int selected)
    {
        audioSource.PlayOneShot(failureAudio[selected]);
    }
}


