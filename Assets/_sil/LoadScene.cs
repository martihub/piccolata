using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class LoadScene : MonoBehaviour
{
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] successWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] motivationWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] introWords;
    [ListDrawerSettings(ShowIndexLabels = true)] public string[] goodbyeWords;


    private void Awake()
    {
        BetterStreamingAssets.Initialize();
    }

    private void Start()
    {


        //string _successWordsRaw = BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/SuccessWords.json");
        //string _motivationRaw = BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/MotivationWords.json");
        string _introRaw = BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/introWords.json");
        //string _goodbyeRaw = BetterStreamingAssets.ReadAllText("Games/_00_Match/Json/GoodbyeWords.json");



        //var _successWords = SimpleJSON.JSON.Parse(_successWordsRaw);
        //var _motivationWords = SimpleJSON.JSON.Parse(_motivationRaw);
        var _introWords = SimpleJSON.JSON.Parse(_introRaw);
        //var _goodbyeWords = SimpleJSON.JSON.Parse(_goodbyeRaw);

        //Debug.Log(_successWords);
        //Debug.Log(_motivationWords);
        Debug.Log(_introWords);

        //successWords = new string[_successWords["word"].Count];
        //motivationWords = new string[_motivationWords["word"].Count];
        introWords = new string[_introWords["word"].Count];
        //goodbyeWords = new string[_goodbyeWords["word"].Count];

        //for (int i = 0; i < _successWords["word"].Count; i++) successWords[i] = _successWords["word"][i];
        //for (int i = 0; i < _motivationWords["word"].Count; i++) motivationWords[i] = _motivationWords["word"][i];
        for (int i = 0; i < _introWords["word"].Count; i++) introWords[i] = _introWords["word"][i];
        //for (int i = 0; i < _goodbyeWords["word"].Count; i++) goodbyeWords[i] = _goodbyeWords["word"][i];
    }



}
