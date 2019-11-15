using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deneme : MonoBehaviour
{
    const string pluginName = "com.ismail.picotorobot.StartCls";
    AndroidJavaClass _pluginClass;
    AndroidJavaObject _pluginInstance;
    public InputField input;
    static AndroidJavaObject context;

    string[] str = new string[5] {
        "A state school in Catalonia ",
        "Fairy tales such as Sleeping Beauty and Little Red Riding Hood were deemed too “toxic” for children under the age of ",
        "One third of the titles",
        "Only one in ten titles could be said to have been written with a positive educational message in terms of “gender perspective”.",
        "A committee analyzed the stories "
    };

    public void PluginStart()
    {
        if (_pluginInstance == null)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = activity.Call<AndroidJavaObject>("getApplicationContext");
            _pluginInstance = (new AndroidJavaClass(pluginName)).CallStatic<AndroidJavaObject>("getInstance", new object[1] { context });
            Debug.Log("______PPPPP");
        }
    }

    int a;
    public void Gonder()
    {
        a++;
        _pluginInstance.Call("deneme", str[a % str.Length]);
        Debug.Log("______GGGGG");
    }
}
