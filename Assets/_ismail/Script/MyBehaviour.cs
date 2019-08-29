
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class MyBehaviour : MonoBehaviour
{
    public string DynamicMessage = "Dynamic error message";

    string url = "http://localhost/games/sphere.dlc";
    string saveTo;
    [InfoBox("$DynamicMessage")]
    public Image img;


    private void Start()
    {
        saveTo = Application.dataPath + "/_ismail/Bundles/_00_Match/Images/images.assetbundle";
        //  StartCoroutine(SaveAndDownload());
        StartCoroutine(ReadeIE());
    }


    IEnumerator SaveAndDownload()
    {
        WWW www = new WWW(url);
        yield return www;
        byte[] bytes = www.bytes;
        File.WriteAllBytes(saveTo, bytes);
    }


    IEnumerator ReadeIE()
    {
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(saveTo);
        yield return uwr.SendWebRequest();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        var loadAsset = bundle.LoadAsset<Sprite>("A");
        img.sprite = loadAsset;

        yield return loadAsset;


    }
}


