using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
public class MatchExtras : MonoBehaviour
{


    public GameObject player;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] successAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] failureAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] successWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] motivationWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] introWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] goodbyeWords;

    Word[] _successWords;
    Word[] _motivationWords;
    Word[] _introWords;
    Word[] _goodbyeWords;
    public Results[] _Results;

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
        _introWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/IntroWords.json");
        _goodbyeWords = GetFromJson<Word>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/GoodbyeWords.json");
        _Results = GetFromJson<Results>.GetArray(Application.streamingAssetsPath + "/Games/_00_Match/Json/SpeechReco.json");

        successWords = new string[_successWords.Length];
        motivationWords = new string[_motivationWords.Length];
        introWords = new string[_introWords.Length];
        goodbyeWords = new string[_goodbyeWords.Length];

        for (int i = 0; i < _successWords.Length; i++) successWords[i] = _successWords[i];
        for (int i = 0; i < _motivationWords.Length; i++) motivationWords[i] = _motivationWords[i];
        for (int i = 0; i < _introWords.Length; i++) introWords[i] = _introWords[i];
        for (int i = 0; i < _goodbyeWords.Length; i++) goodbyeWords[i] = _goodbyeWords[i];

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

    public string GetSpeech(TalkType _talkType)
    {
        switch (_talkType)
        {
            case TalkType.INTRO:
                return introWords[Random.Range(0, introWords.Length)];
            case TalkType.SUCCESS:
                return successWords[Random.Range(0, successWords.Length)];
            case TalkType.MOTIVATION:
                return motivationWords[Random.Range(0, motivationWords.Length)];
            case TalkType.GOODBYE:
                return goodbyeWords[Random.Range(0, goodbyeWords.Length)];
            default:
                return "Hello. No answer!";
        }
    }
}

public enum TalkType
{
    INTRO,
    SUCCESS,
    MOTIVATION,
    GOODBYE
}
