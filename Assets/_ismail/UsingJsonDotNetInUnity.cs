using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEditor;

public class UsingJsonDotNetInUnity : MonoBehaviour
{
    public BehaviorTree bt;
    public Account acc;
    private void Start()
    {
        var path = Application.streamingAssetsPath + "/TestJson.json";
        var fileContent = File.ReadAllText(path);
        //var accountsFromFile = JsonConvert.DeserializeObject<List<Account>>(fileContent);
        acc = JsonConvert.DeserializeObject<Account>(fileContent);
        // StartCoroutine(dene());
        bt.SetVariableValue("pos", Vector3.zero);

    }

    [System.Serializable]
    public class Account
    {
        //public string Email;
        //public bool Active;
        //public DateTime CreatedDate;
        //public IList<string> Roles;
        public Vector3 Ve;
        //public Dictionary<string, Vector3> StrVector3Dictionary;
    }



}