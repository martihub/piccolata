using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;


/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android, or platform that supports mp3 files. To test, change the platform to Android.</explanation>
/// </summary>

public class TestTTS : MonoBehaviour
{
    public static string sound_text;
    public Text text;

    public AudioSource _audio;

    void Start()
    {
       // _audio = gameObject.GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (MainAudioRecognition.speechOut)
        {
            Debug.Log("TTS");
            text.text = "TTS started";
            if(MainAudioRecognition.lang == "English")
                StartCoroutine(DownloadTheAudio(sound_text, "English"));
            else
                StartCoroutine(DownloadTheAudio(sound_text, "German"));
            MainAudioRecognition.speechOut = false;
        }

    }
    IEnumerator DownloadTheAudio(string soundtext, string lang)
    {
        Debug.Log("TTS started");
        string url;
        if (lang == "English")
        {
            text.text = "TTS started English";
            url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
            + soundtext + "&tl=En-gb";
        }
        else
        {
            text.text = "TTS started German";
            url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q="
            + soundtext + "&tl=De-de";
        }
        
        WWW www = new WWW(url);
        yield return www;
        _audio.clip = www.GetAudioClip(false, false, AudioType.MPEG);
        _audio.Play();
        text.text = "TTS stopped";
    }
}
