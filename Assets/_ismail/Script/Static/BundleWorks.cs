using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class BundleWorks
{
    public static T GetObject<T>(GameType _gameType, BundleType _bundleType, string _bundleName, string _gameObjectName) where T : Object
    {
        string bundlePath = GetBundleTypePath(_gameType, _bundleType) + _bundleName + ".assetbundle";
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlePath);
        T gObj = myLoadedAssetBundle.LoadAsset<T>(_gameObjectName);
        myLoadedAssetBundle.Unload(false);
        return gObj;
    }

    public static void SetBundleLists(GameType _gameType, out AllBundles[] _allAssetBundles)
    {
        var jsonToStr = GetJsonToStr(GetGameTypePath(_gameType) + "Assets.json");
        List<string> bundleTitles = new List<string>();
        foreach (var item in jsonToStr.Keys) bundleTitles.Add(item);
        _allAssetBundles = new AllBundles[jsonToStr.Count];
        for (int i = 0; i < _allAssetBundles.Length; i++)
        {
            _allAssetBundles[i].bundles = new bundle[jsonToStr[i].Count];
        }
        for (int i = 0; i < jsonToStr.Count; i++)
        {
            _allAssetBundles[i].Name = bundleTitles[i];
            for (int j = 0; j < jsonToStr[i].Count; j++)
            {
                _allAssetBundles[i].bundles[j].Name = jsonToStr[i][j]["Name"];
                _allAssetBundles[i].bundles[j].Version = jsonToStr[i][j]["Version"].AsInt;
            }
        }
    }

    public static void SetBundleLists(string _json, out AllBundles[] _allAssetBundles)
    {
        var jsonToStr = SimpleJSON.JSON.Parse(_json);
        List<string> bundleTitles = new List<string>();
        foreach (var item in jsonToStr.Keys) bundleTitles.Add(item);

        _allAssetBundles = new AllBundles[jsonToStr.Count];
        for (int i = 0; i < _allAssetBundles.Length; i++)
        {
            _allAssetBundles[i].bundles = new bundle[jsonToStr[i].Count];
        }
        for (int i = 0; i < jsonToStr.Count; i++)
        {
            _allAssetBundles[i].Name = bundleTitles[i];
            for (int j = 0; j < jsonToStr[i].Count; j++)
            {
                _allAssetBundles[i].bundles[j].Name = jsonToStr[i][j]["Name"];
                _allAssetBundles[i].bundles[j].Version = jsonToStr[i][j]["Version"].AsInt;
            }
        }
    }

    public static T[] GetRandomAssets<T>(GameType _gameType, BundleType _bundleType) where T : Object
    {
        List<string> bundleTitles = new List<string>();
        List<string> bundleList = new List<string>();
        AllBundles[] allBundles;
        SetBundleLists(_gameType, out allBundles);

        foreach (var item in allBundles)
        {
            if (item.Name == _bundleType.ToString())
            {
                foreach (var item2 in item.bundles)
                {
                    bundleList.Add(item2.Name);
                }
            }
        }
        int selected = Random.Range(0, bundleList.Count);
        string bundlePath = GetBundleTypePath(_gameType, _bundleType) + bundleList[selected] + ".assetbundle";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
        string[] objectName = GetOnlyName(assetBundle);
        assetBundle.Unload(false);
        T[] objectArray = new T[objectName.Length];
        for (int i = 0; i < objectArray.Length; i++)
        {
            objectArray[i] = GetObject<T>(_gameType, _bundleType, bundleList[selected], objectName[i]);
        }
        return objectArray;
    }

    public static SimpleJSON.JSONNode GetJsonToStr(string _path)
    {
        string jsonRaw = File.ReadAllText(_path);
        var jsonToStr = SimpleJSON.JSON.Parse(jsonRaw);
        return jsonToStr;
    }

    public static string GetGameTypePath(GameType _gameType)
    {
        return Application.dataPath + "/_ismail/Bundles/" + _gameType + "/";
    }

    public static string GetBundleTypePath(GameType _gameType, BundleType _assetBundleType)
    {
        return Application.dataPath + "/_ismail/Bundles/" + _gameType + "/" + _assetBundleType + "/";
    }

    public static string[] GetOnlyName(AssetBundle assetBundle)
    {
        string[] str = assetBundle.GetAllAssetNames();
        for (int i = 0; i < str.Length; i++)
        {
            str[i] = Path.GetFileName(str[i]);
            // str[i] = Path.GetFileNameWithoutExtension(str[i]);
        }
        return str;
    }

    public static string GetFileExt<T>() where T : Object
    {
        if (typeof(T) == typeof(Sprite))
        {
            return ".png";
        }
        else if (typeof(T) == typeof(GameObject))
        {
            return ".prefab";
        }
        return null;
    }
}

// Json     ->  Name
// Bundle   ->  ObjectList