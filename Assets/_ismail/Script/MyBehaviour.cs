
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class MyBehaviour : MonoBehaviour
{
    private void Start()
    {
        GameObject g = GameobjectInstantiator.Instantiate(GameType._00_Match, AssetBundleType._00_BkgGameObjects, "00", "BgkGameobject");
    }

}


