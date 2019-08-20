using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : MonoBehaviour
{
    float sendTreshold = .001f;
    public float MicLoudness;
    private string _device;
    public bool isStarted;
    AudioClip _clipRecord;
    int _sampleWindow = 128;
    bool _isInitialized;
    public static MicInput instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Update()
    {
        MicLoudness = LevelMax();
        if (!isStarted)
        {
            if (MicLoudness > sendTreshold)
            {
                isStarted = true;
                StopMicrophone();
                GoogleVoiceSpeech.instance.MicStart();
            }
        }
    }

    public void RestartMic()
    {
        isStarted = false;
        InitMic();
    }

    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }
}
