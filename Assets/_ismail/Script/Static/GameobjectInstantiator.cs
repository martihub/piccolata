using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameobjectInstantiator
{
    public static GameObject Instantiate(GameType _gameType, AssetBundleType _assetBundleType, string _bundleName, string _gameObjectName)
    {
        string bundlesPath = Application.dataPath + "/_ismail/Bundles/" + _gameType + "/" + _assetBundleType + "/" + _bundleName + ".assetbundle";
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlesPath);
        var gObj = myLoadedAssetBundle.LoadAsset<GameObject>(_gameObjectName);
        GameObject g = GameObject.Instantiate(gObj);
        g.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
        myLoadedAssetBundle.Unload(false);
        return g;
    }

}
