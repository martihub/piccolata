using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MicControlD : MonoBehaviour
{

    public float micLevel;
    public bool isStarted;
    float sendTreshold = .1f;
    public static MicControlD instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (micLevel > sendTreshold && !isStarted)
        {
            isStarted = true;
            GoogleVoiceSpeech.instance.MicStopAndSend();
        }
    }

    public void ResetMic()
    {
        isStarted = false;
        // MicControlC.instance.StartMicrophone();
    }
}
