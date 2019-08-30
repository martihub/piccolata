
using UnityEngine;
using System;
using UnityEngine.UI;

public class MyBehaviour : MonoBehaviour
{
    public Image img, img2;
    GameObject g;
    public string[] str;

    public Sprite[] sprites;
    public GameObject[] gObjs;

    private void Start()
    {
        //g = BundleWorks.GetObject<GameObject>(GameType._00_Match, BundleType._00_BkgGameObjects, "00", "bgkgameobject");
        //Instantiate<GameObject>(g);
        //g.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Standard");
        //img.sprite = BundleWorks.GetObject<Sprite>(GameType._00_Match, BundleType._01_Images, "00", "a");
        //img2.sprite = BundleWorks.GetObject<Sprite>(GameType._00_Match, BundleType._01_Images, "00", "b");

        sprites = BundleWorks.GetRandomAssets<Sprite>(GameType._00_Match, BundleType._01_Images);
        gObjs = BundleWorks.GetRandomAssets<GameObject>(GameType._00_Match, BundleType._00_BkgGameObjects);
        g = Instantiate(gObjs[0]);
        g.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Standard");
    }

    public Type GetService<T>() where T : UnityEngine.Object
    {
        // T t = new T();
        return typeof(T);
    }
}


