using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using Better.StreamingAssets;
using UnityEngine.UI;
public class MatchExtras : MonoBehaviour
{
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] successAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public AudioClip[] failureAudio;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] successWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] motivationWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] introWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] goodbyeWords;

    AudioSource audioSource;
    public static MatchExtras instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BetterStreamingAssets.Initialize();

        var _successWords = SimpleJSON.JSON.Parse(BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/SuccessWords.json"));
        var _motivationWords = SimpleJSON.JSON.Parse(BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/MotivationWords.json"));
        var _introWords = SimpleJSON.JSON.Parse(BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/introWords.json"));
        var _goodbyeWords = SimpleJSON.JSON.Parse(BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/GoodbyeWords.json"));

        successWords = new string[_successWords["word"].Count];
        motivationWords = new string[_motivationWords["word"].Count];
        introWords = new string[_introWords["word"].Count];
        goodbyeWords = new string[_goodbyeWords["word"].Count];

        for (int i = 0; i < _successWords["word"].Count; i++) successWords[i] = _successWords["word"][i];
        for (int i = 0; i < _motivationWords["word"].Count; i++) motivationWords[i] = _motivationWords["word"][i];
        for (int i = 0; i < _introWords["word"].Count; i++) introWords[i] = _introWords["word"][i];
        for (int i = 0; i < _goodbyeWords["word"].Count; i++) goodbyeWords[i] = _goodbyeWords["word"][i];

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
