using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Networking;
/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android, or platform that supports mp3 files. To test, change the platform to Android.</explanation>
/// </summary>

public class TestTTS1 : MonoBehaviour
{
    //public static string sound_text;
    public Text text;
    AudioSource _audio;
    private float time = 0.0f;

    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(DownloadTheAudio());
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 30.0f)
        {
            StartCoroutine(DownloadTheAudio());
            time = 0.0f;
        }

        //StartCoroutine(DownloadTheAudio());
        //if (MainAudioRecognition.speechOut)
        //{
        //    Debug.Log("TTS");
        //    text.text = "TTS started";
        //    if(MainAudioRecognition.lang == "English")
        //        StartCoroutine(DownloadTheAudio(sound_text, "English"));
        //    else
        //        StartCoroutine(DownloadTheAudio(sound_text, "German"));
        //    MainAudioRecognition.speechOut = false;
        //}

    }
    IEnumerator DownloadTheAudio()
    {
        Debug.Log("TTS started");
        string url;
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + "Hello, how are you?" + "&tl=En-gb"; ;
        //if (lang == "English")
        //{
        //    text.text = "TTS started English";
        //    url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
        //    + soundtext + "&tl=En-gb";
        //}
        //else
        //{
        //    text.text = "TTS started German";
        //    url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
        //    + soundtext + "&tl=De-de";
        //}
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        
            yield return www.Send();

            if (www.isNetworkError)
            {
                text.text = www.error;
                Debug.Log(www.error);
            }
            else
            {
                AudioSource _audio = GetComponent<AudioSource>();
                _audio.clip = DownloadHandlerAudioClip.GetContent(www);
                var filePath = Path.Combine("testing/", "tts" + time + ".wav");
                filePath = Path.Combine(Application.persistentDataPath, filePath);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                //Save a temporary Wav File

                yield return new WaitForSeconds(10);
                _audio.Play();

                Debug.Log(SavWav.Save(filePath, _audio.clip));
                text.text = url;
        }

        //WWW www = new WWW(url);
        //yield return www;
        //AudioSource _audio = GetComponent<AudioSource>();
        //_audio.clip = www.GetAudioClip(false, false, AudioType.MPEG);

        //var filePath = Path.Combine("testing/", "tts"+time+".wav");
        //filePath = Path.Combine(Application.persistentDataPath, filePath);
        //Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        // //Save a temporary Wav File

        //yield return new WaitForSeconds(10);
        //_audio.Play();

        //Debug.Log(SavWav.Save(filePath, _audio.clip));
        //text.text = url;
    }
}

