using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetDownloader : MonoBehaviour
{

    string _url = "http://localhost/";
    string url;
    public GameType gameType;
    // public string[] assets;
    public AssetSubFolder[] assetSubFolders;


    void Start()
    {
        url = _url + gameType + "/" + "Asset.json";
        StartCoroutine(GetText());

        //  StartCoroutine(SaveDownloaded());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            SetAssets(www.downloadHandler.text);
        }
    }

    void SetAssets(string _str)
    {
        var str = SimpleJSON.JSON.Parse(_str);
        assetSubFolders = new AssetSubFolder[str[0].Count];

        for (int i = 0; i < str["AllFolders"].Count; i++)
        {
            assetSubFolders[i].Assets = new string[str["AllFolders"][i]["Assets"].Count];
        }

        for (int i = 0; i < str[0].Count; i++)
        {
            assetSubFolders[i].Name = str["AllFolders"][i]["Name"];
            assetSubFolders[i].Version = str["AllFolders"][i]["Version"].AsInt;
            for (int j = 0; j < str["AllFolders"][i]["Assets"].Count; j++)
            {
                assetSubFolders[i].Assets[j] = str["AllFolders"][i]["Assets"][j];
            }
        }
    }

    //IEnumerator SaveDownloaded()
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://localhost/_00_Match/00_1.png");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        SaveTextureAsPNG(myTexture, Application.streamingAssetsPath + "/Games/_00_Match/" + "img.png");
    //    }
    //}


    //public void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    //{
    //    byte[] _bytes = _texture.EncodeToPNG();
    //    System.IO.File.WriteAllBytes(_fullPath, _bytes);
    //    Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    //}
}
