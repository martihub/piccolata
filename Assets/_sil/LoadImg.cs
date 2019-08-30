using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadImg : MonoBehaviour
{



    Shader standardShader;

    void Start()
    {
        standardShader = Shader.Find("Standard");
        changeShader();
    }

    void changeShader() // because shadow for assetbundle is cucked.
    {
        var renderers = FindObjectsOfType<Renderer>() as Renderer[];
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.shader = standardShader;
    }


}
