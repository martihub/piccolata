using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class MatchExtras : MonoBehaviour
{


    public GameObject player;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] successAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] failureAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] successWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] motivationWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] scenarioWords;

    Word[] _successWords;
    Word[] _motivationWords;
    Word[] _scenarioWords;
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
        _successWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/SuccessWords.json");
        _motivationWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/MotivationWords.json");
        _scenarioWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/ScenarioWords.json");
        successWords = new string[_successWords.Length];
        motivationWords = new string[_motivationWords.Length];
        scenarioWords = new string[_motivationWords.Length];

        for (int i = 0; i < _successWords.Length; i++) successWords[i] = _successWords[i];
        for (int i = 0; i < _motivationWords.Length; i++) motivationWords[i] = _motivationWords[i];
        for (int i = 0; i < _scenarioWords.Length; i++) scenarioWords[i] = _scenarioWords[i];

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


