using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashImageResize : MonoBehaviour
{
    // Start is called before the first frame update
    //scale the splash image fit to the screensize
    void Start()
    {
        Vector2 myVector = new Vector2(Screen.width, Screen.height);
        transform.gameObject.GetComponent<RectTransform>().sizeDelta = myVector;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 myVector = new Vector2(Screen.width, Screen.height);
        transform.gameObject.GetComponent<RectTransform>().sizeDelta = myVector;
    }
}
