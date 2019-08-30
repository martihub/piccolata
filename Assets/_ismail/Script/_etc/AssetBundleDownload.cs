using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class AssetBundleDownload : MonoBehaviour
{

    string webLink;
    string webJson;
    public GameType gameType;
    public AllAssetbundles[] allLocalAssetBundles;
    public AllAssetbundles[] allWebAssetBundles;
    string streamingAssetsPath;
    string allBundlesPath;
    List<string> bundleTitles = new List<string>();

    void Start()
    {
        webLink = "http://www.piccolata.com/update/" + gameType + "/";
        // webLink = "http://localhost/games/" + gameType + "/";
        Debug.Log(webLink);
        allBundlesPath = Application.dataPath + "/_ismail/Bundles/" + gameType + "/";
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
            SetAssetLists(www.downloadHandler.text, out allWebAssetBundles);
            GetLocelJson();
            string _webJson = www.downloadHandler.text;
            File.WriteAllText(allBundlesPath + "Assets.json", _webJson);
        }
        CompareTwoAssetsAndCreateDir();
    }

    void GetLocelJson()
    {
        string path = allBundlesPath + "Assets.json";
        string json = File.ReadAllText(path);
        SetAssetLists(json, out allLocalAssetBundles);
    }

    void SetAssetLists(string _json, out AllAssetbundles[] _allAssetBundles)
    {
        var str = SimpleJSON.JSON.Parse(_json);
        _allAssetBundles = new AllAssetbundles[str.Count];
        for (int i = 0; i < _allAssetBundles.Length; i++)
        {
            _allAssetBundles[i].Assetbundle = new Assetbundle[str[i].Count];
        }

        for (int i = 0; i < str.Count; i++)
        {
            _allAssetBundles[i].Name = bundleTitles[i];
            for (int j = 0; j < str[i].Count; j++)
            {
                _allAssetBundles[i].Assetbundle[j].Name = str[i][j]["Name"];
                _allAssetBundles[i].Assetbundle[j].Version = str[i][j]["Version"].AsInt;
            }
        }
    }

    void CompareTwoAssetsAndCreateDir()
    {
        for (int i = 0; i < allWebAssetBundles.Length; i++)
        {
            for (int j = 0; j < allWebAssetBundles[i].Assetbundle.Length; j++)
            {
                try
                {
                    if (allLocalAssetBundles[i].Assetbundle[j].Version < allWebAssetBundles[i].Assetbundle[j].Version)
                    {
                        StartCoroutine(SaveAndDownload(bundleTitles[i], allWebAssetBundles[i].Assetbundle[j].Name));
                    }

                }
                catch (System.Exception)
                {
                    StartCoroutine(SaveAndDownload(bundleTitles[i], allWebAssetBundles[i].Assetbundle[j].Name));
                }
            }
        }
    }

    IEnumerator SaveAndDownload(string _bundleTitle, string _bundleName)
    {
        string shortExt = _bundleTitle + "/" + _bundleName + ".assetbundle";
        string downLink = webLink + shortExt;
        string bundlePath = allBundlesPath + shortExt;
        if (!Directory.Exists(allBundlesPath + _bundleTitle))
        {
            Directory.CreateDirectory(allBundlesPath + _bundleTitle);
        }
        WWW www = new WWW(downLink);
        yield return www;
        byte[] bytes = www.bytes;
        File.WriteAllBytes(bundlePath, bytes);
    }

    void deneIE()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/_ismail/Bundles/" + gameType + "/_01_Images/" + "00.assetbundle");
        var spr = myLoadedAssetBundle.LoadAsset<Sprite>("A");
        myLoadedAssetBundle.Unload(false);
    }
}

