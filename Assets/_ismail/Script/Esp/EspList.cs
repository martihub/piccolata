using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspList : MonoBehaviour
{
    const string pluginName = "com.alimurat.esplistlib.EspListCls";
    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;

    public static EspList instance;


    void Start()
    {
        if (instance == null) instance = this;
        _pluginClass = new AndroidJavaClass(pluginName);
        _pluginInstance = _pluginClass.CallStatic<AndroidJavaObject>("getInstance");
        // StartCoroutine(GetListFromAndroidIE());
    }

    // Update is called once per frame
    string espList;
    string str;
    string[] arr;
    IEnumerator GetListFromAndroidIE()
    {
        yield return new WaitForSeconds(2);
        espList = _pluginInstance.Call<string>("GetEspList");
        espList = espList.Remove(espList.Length - 1);
        arr = espList.Split(' ');

        foreach (var item in arr)
        {
            Debug.Log("___" + item);
        }
        Debug.Log("___" + arr.Length);
        GetComponent<Text>().text = str;
    }

    public void SendParameterToEsp()
    {
        _pluginInstance.Call("SendParameterToEsp");
    }

}
