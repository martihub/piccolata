using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspMono : MonoBehaviour
{
    const string pluginName = "com.ismail.picotorobot.StartCls";
    AndroidJavaClass _pluginClass;
    AndroidJavaObject _pluginInstance;
    static AndroidJavaObject context;
    bool isSend;

    public void Start()
    {
        if (_pluginInstance == null)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = activity.Call<AndroidJavaObject>("getApplicationContext");
            _pluginInstance = (new AndroidJavaClass(pluginName)).CallStatic<AndroidJavaObject>("getInstance", new object[1] { context });
        }
    }


    public void Gonder(string str)
    {
        if (!isSend)
        {
            _pluginInstance.Call("nextVoice", str);
            isSend = true;
            StartCoroutine(SendIE());
        }
    }

    IEnumerator SendIE()
    {
        yield return new WaitForSeconds(1);
        isSend = false;

    }

}
