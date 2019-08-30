using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadImg : MonoBehaviour
{

    public Image img;


    IEnumerator Start()
    {


        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "00.assetbundle"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield return null;
        }

        var spr = myLoadedAssetBundle.LoadAsset<Sprite>("A");
        img.sprite = spr;
        myLoadedAssetBundle.Unload(false);

        //yield return bundleLoadRequest;

        //var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        //if (myLoadedAssetBundle == null)
        //{
        //    Debug.Log("Failed to load AssetBundle!");
        //    yield break;
        //}

        //var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<Sprite>("A");
        //yield return assetLoadRequest;

        //Sprite spr = assetLoadRequest.asset as Sprite;

        //myLoadedAssetBundle.Unload(false);


    }


}
