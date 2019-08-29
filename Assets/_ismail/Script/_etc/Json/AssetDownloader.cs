using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class AssetDownloader : MonoBehaviour
{

    string webLink;
    string jsonUrl;
    public GameType gameType;
    public AssetSubFolder[] localAssetSubFolders;
    public AssetSubFolder[] webAssetSubFolders;
    string streamingAssetsPath;
    string webJson;

    void Start()
    {
        Debug.Log(Application.dataPath);
        //webLink = "http://localhost/games/" + gameType;
        webLink = "http://www.piccolata.com/update/" + gameType;
        streamingAssetsPath = Application.streamingAssetsPath + "/games/" + gameType;
        GetLocelJson();
        StartCoroutine(GetWebJsonIE());

        string str = "12";
        int a = str.ToInt();
        Debug.Log(a);
    }

    IEnumerator GetWebJsonIE()
    {
        jsonUrl = webLink + "/Assets.json";
        UnityWebRequest www = UnityWebRequest.Get(jsonUrl);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            SetAssetLists(www.downloadHandler.text, ref webAssetSubFolders);
            webJson = www.downloadHandler.text;
            File.WriteAllText(streamingAssetsPath + " /Assets.json", webJson);
        }
        CompareTwoAssetsAndCreateDir();
    }

    void GetLocelJson()
    {
        string path = streamingAssetsPath + "/Assets.json";
        string json = File.ReadAllText(path);
        SetAssetLists(json, ref localAssetSubFolders);
    }

    void SetAssetLists(string _json, ref AssetSubFolder[] _assetSubFolders)
    {
        var str = SimpleJSON.JSON.Parse(_json);
        _assetSubFolders = new AssetSubFolder[str[0].Count];
        for (int i = 0; i < str["AllFolders"].Count; i++)
        {
            _assetSubFolders[i].Assets = new string[str["AllFolders"][i]["Assets"].Count];
        }
        for (int i = 0; i < str[0].Count; i++)
        {
            _assetSubFolders[i].Name = str["AllFolders"][i]["Name"];
            _assetSubFolders[i].Version = str["AllFolders"][i]["Version"].AsInt;
            for (int j = 0; j < str["AllFolders"][i]["Assets"].Count; j++)
            {
                _assetSubFolders[i].Assets[j] = str["AllFolders"][i]["Assets"][j];
            }
        }
    }

    void CompareTwoAssetsAndCreateDir()
    {
        int difference = webAssetSubFolders.Length - localAssetSubFolders.Length;
        if (difference != 0)
        {
            for (int i = 0; i < difference; i++)
            {
                int a = localAssetSubFolders.Length + i;
                string s = $"{a:D2}";
                Directory.CreateDirectory(streamingAssetsPath + "/" + s);
                DownloadAllSubfolderContent(s);
            }
        }
    }

    void DownloadAllSubfolderContent(string _subfolder)
    {
        int a = _subfolder.ToInt();
        string[] subFolder = webAssetSubFolders[a].Assets;
        foreach (var asset in subFolder)
        {
            StartCoroutine(SaveDownloaded(_subfolder, asset));
        }

    }

    IEnumerator SaveDownloaded(string _subFolder, string _asset)
    {
        string assetLink = webLink + "/" + _subFolder + "/" + _asset;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(assetLink);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            SaveTextureAsPNG(myTexture, streamingAssetsPath + "/" + _subFolder + "/" + _asset);
        }
    }

    public void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        File.WriteAllBytes(_fullPath, _bytes);
    }
}
