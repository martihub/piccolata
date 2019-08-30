using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetBundleDownload : MonoBehaviour
{

    string webLink;
    string webJson;
    public GameType gameType;
    public AllBundles[] allLocalBundles;
    public AllBundles[] allWebBundles;
    string gameTypePath;
    List<string> bundleTitles = new List<string>();

    void Start()
    {
        webLink = "http://www.piccolata.com/update/" + gameType + "/";
        // webLink = "http://localhost/games/" + gameType + "/";
        gameTypePath = BundleWorks.GetGameTypePath(gameType);
        StartCoroutine(GetWebJsonIE());
    }

    IEnumerator GetWebJsonIE()
    {
        webJson = webLink + "Assets.json";
        UnityWebRequest www = UnityWebRequest.Get(webJson);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var str = SimpleJSON.JSON.Parse(www.downloadHandler.text);
            foreach (var item in str.Keys)
            {
                bundleTitles.Add(item);
            }
            BundleWorks.SetBundleLists(www.downloadHandler.text, out allWebBundles);
            BundleWorks.SetBundleLists(gameType, out allLocalBundles);// Get Local Json
            string _webJson = www.downloadHandler.text;
            File.WriteAllText(gameTypePath + "Assets.json", _webJson);
        }
        CompareTwoAssets();
    }

    void CompareTwoAssets()
    {
        for (int i = 0; i < allWebBundles.Length; i++)
        {
            for (int j = 0; j < allWebBundles[i].bundles.Length; j++)
            {
                try
                {
                    if (allLocalBundles[i].bundles[j].Version < allWebBundles[i].bundles[j].Version)
                    {
                        StartCoroutine(DownloadAndSave(bundleTitles[i], allWebBundles[i].bundles[j].Name));
                    }

                }
                catch (System.Exception)
                {
                    StartCoroutine(DownloadAndSave(bundleTitles[i], allWebBundles[i].bundles[j].Name));
                }
            }
        }
    }

    IEnumerator DownloadAndSave(string _bundleTitle, string _bundleName)
    {
        string shortExt = _bundleTitle + "/" + _bundleName + ".assetbundle";
        string downLink = webLink + shortExt;
        string bundlePath = gameTypePath + shortExt;
        if (!Directory.Exists(gameTypePath + _bundleTitle))
        {
            Directory.CreateDirectory(gameTypePath + _bundleTitle);
        }
        WWW www = new WWW(downLink);
        yield return www;
        byte[] bytes = www.bytes;
        File.WriteAllBytes(bundlePath, bytes);
    }
}

