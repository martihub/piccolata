using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    public static int player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickAligetter()
    {
        player = 0;
    }

    public void clickCaptain()
    {
        player = 1;
    }

    public void clickPiko()
    {
        player = 2;
    }
}
