using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class UsingJsonDotNetInUnity : MonoBehaviour
{
    private void Awake()
    {
        var accountJames = new Account
        {
            Email = "james@example.com",
            Active = true,
            CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
            Roles = new List<string>
            {
                "User",
                "Admin"
            },
            Ve = new Vector3(10, 3, 1),
            StrVector3Dictionary = new Dictionary<string, Vector3>
            {
                {"start", new Vector3(0, 0, 1)},
                {"end", new Vector3(9, 0, 1)}
            }
        };


        var accountOnion = new Account
        {
            Email = "onion@example.co.uk",
            Active = true,
            CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
            Roles = new List<string>
            {
                "User",
                "Admin"
            },
            Ve = new Vector3(0, 3, 1),
            StrVector3Dictionary = new Dictionary<string, Vector3>
            {
                {"vr", new Vector3(0, 0, 1)},
                {"pc", new Vector3(9, 9, 1)}
            }
        };


        var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        // write
        var accountsFromCode = new List<Account> { accountJames, accountOnion };
        var json = JsonConvert.SerializeObject(accountsFromCode, setting);
        var path = Path.Combine(Application.dataPath, "hi.json");
        File.WriteAllText(path, json);

        // read
        var fileContent = File.ReadAllText(path);
        var accountsFromFile = JsonConvert.DeserializeObject<List<Account>>(fileContent);
        var reSerializedJson = JsonConvert.SerializeObject(accountsFromFile, setting);

        print(reSerializedJson);
        print("json == reSerializedJson is" + (json == reSerializedJson));
    }

    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
        public Vector3 Ve { get; set; }
        public Dictionary<string, Vector3> StrVector3Dictionary { get; set; }
    }
}