using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashButton : MonoBehaviour
{
    // Start is called before the first frame update
    //scale the button size fit to the screensize
    void Start()
    {
        Vector2 myVector = new Vector2(Screen.width/5, Screen.height/6);
        Vector2 myVector1 = new Vector2(0, Screen.height / 5);
        transform.gameObject.GetComponent<RectTransform>().sizeDelta = myVector;
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = myVector1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 myVector = new Vector2(Screen.width/5, Screen.height/6);
        Vector2 myVector1 = new Vector2(0, Screen.height / 5);
        transform.gameObject.GetComponent<RectTransform>().sizeDelta = myVector;
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = myVector1;
    }

    //if click the start button, go to the select character scene
    public void ButtonClick()
    {
        //SceneManager.LoadScene("SelectCharacter");
        SceneManager.LoadScene("MainGame");
    }
}
