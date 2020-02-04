using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class Example : MonoBehaviour
{




    public void Sifir()
    {
        StartCoroutine(SendParam(0));
    }

    public void Bir()
    {
        StartCoroutine(SendParam(1));
    }

    IEnumerator SendParam(int _param)
    {
        string str = "http://192.168.1.105/?param2=" + _param.ToString();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(str))
        {
            yield return webRequest.SendWebRequest();
        }
    }

}