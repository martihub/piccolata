using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGoogleWords : MonoBehaviour
{

    public static ManageGoogleWords instance;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private string words;
    public string SetWords {
        get { return words; }
        set {
            words = value;
            Debug.Log(words);
        }
    }

}
