using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void Asset_0()
    {
        // string str = Application.persistentDataPath + "/Assets/_ismail/Bundles/" + "_00_Match" + "/" + "_00_BkgGameObjects" + "/" + "00.assetbundle";
        // string str = BundleWorks.GetBundleTypePath(GameType._00_Match, BundleType._00_BkgGameObjects) + "/" + "00.assetbundle";
        GameObject enviro = Instantiate(BundleWorks.GetObject<GameObject>(GameType._00_Match, BundleType._00_BkgGameObjects, "00", "BgkGameobject"));
        //var myLoadedAssetBundle = AssetBundle.LoadFromFile(str);
        //GameObject gObj = myLoadedAssetBundle.LoadAsset<GameObject>("BgkGameobject");
        //Instantiate(gObj);
        //myLoadedAssetBundle.Unload(false);
    }


    public void Asset_1()
    {
        SceneManager.LoadScene("00_Match");
    }

    //Application.persistentDataPath + "/../Assets
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
