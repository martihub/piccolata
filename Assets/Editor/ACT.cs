using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ACT
{

    [MenuItem("GameObject/ACT &q", false, 0)]
    static void Init()
    {
        foreach (var item in Selection.gameObjects)
        {
            item.SetActive(!item.activeSelf);
            foreach (Transform tr in item.transform)
            {
                tr.gameObject.SetActive(!tr.gameObject.activeSelf);
            }

        }

    }
}