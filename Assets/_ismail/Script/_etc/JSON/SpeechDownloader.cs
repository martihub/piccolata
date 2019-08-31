
using UnityEngine;

using System.Collections;


public static class SpeechDownloader
{
    public static IEnumerator DownloadTheAudio(AudioSource audioSource, string speech)
    {
        string url;
        url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=" + speech + "&tl=en-US";
        WWW www = new WWW(url);
        yield return www;
        audioSource.clip = www.GetAudioClip(false, false, AudioType.MPEG);
        audioSource.Play();
        Debug.Log("GELDİ");
    }
}
