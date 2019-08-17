using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEditor;
using System.Collections.Generic;
using UnityRest;

public class JsonParser : MonoBehaviour
{
    public BehaviorTree bt;
    public CharIntro[] acc;
    private void Start()
    {
        acc = GetFromJson<CharIntro>.GetArray(Application.streamingAssetsPath + "/_etc/TestJson.json");

        bt.SetVariableValue("Pos", acc[0].Pos);
        bt.SetVariableValue("Action", acc[0].Action);
        bt.SetVariableValue("Talk0", acc[0].Talk0);
    }






}