using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEditor;
using System.Collections.Generic;


public class JsonParser : MonoBehaviour
{
    public BehaviorTree bt;
    public Account acc;
    private void Start()
    {
        var path = Application.streamingAssetsPath + "/TestJson.json";
        var fileContent = File.ReadAllText(path);
        acc = JsonConvert.DeserializeObject<Account>(fileContent);
        bt.SetVariableValue("Pos", acc.Pos);
        bt.SetVariableValue("Action", acc.Action);
        bt.SetVariableValue("Talk0", acc.Talk0);
    }

    [System.Serializable]
    public class Account
    {
        public Vector3 Pos;
        public string Action;
        public string Talk0;
    }




}