using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using SerializableTypes;
using UnityRest;

public class CharacterSpawner : MonoBehaviour
{

    public Character[] characters;
    void Start()
    {
        //var path = Application.streamingAssetsPath + "/TestJson.json";
        //var fileContent = File.ReadAllText(path);
        //weaponList = JsonHelper.FromJson<Character>(fileContent);
        //characters = JsonConvert.DeserializeObject<List<Character>>(fileContent);
        //  Character ch = JsonConvert.DeserializeObject<Character>(fileContent);
        //   Debug.Log(ch.Name);
        CreateCharacters();
    }

    void CreateCharacters()
    {
        //for (int i = 0; i < characters.Count; i++)
        //{
        //    string name = characters[i].Name;
        //    Debug.Log(name);
        //    GameObject g = (GameObject)Instantiate(Resources.Load(name));
        //    g.transform.position = characters[i].Pos;
        //    g.SetActive(true);
        //}

        var path = Application.streamingAssetsPath + "/TestJson.json";
        var fileContent = File.ReadAllText(path);
        // string dataText = data.ToString();
        characters = JsonHelper.FromJson<Character>(fileContent);

    }
}

[System.Serializable]
public class Character
{
    public string Name;
    public string Level;
    public Vector3 Pos;
}

