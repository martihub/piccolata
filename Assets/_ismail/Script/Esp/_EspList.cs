//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class EspList : MonoBehaviour
//{

//    static AndroidJavaClass _espListPluginClass;
//    static AndroidJavaObject _espListPluginInstance;
//    const string espPluginName = "com.alimurat.esplistlib.EspListCls";

//    void Start()
//    {
//        _espListPluginClass = new AndroidJavaClass(espPluginName);
//        _espListPluginInstance = _espListPluginClass.CallStatic<AndroidJavaObject>("getInstance");
//        StartCoroutine(Deneme());
//    }

//    string espLine;
//    string str;
//    string[] espList;

//    IEnumerator Deneme()
//    {
//        yield return new WaitForSeconds(2);
//        espLine = _espListPluginInstance.Call<string>("GetEspList");
//        espLine = espLine.Remove(espLine.Length - 1);
//        espList = espLine.Split(' ');
//    }
//}
