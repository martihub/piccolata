using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class CharacterSpawner : MonoBehaviour
{

    public List<Character> characters;
    void Start()
    {
        var path = Application.streamingAssetsPath + "/CharacterSpawnList.json";
        var fileContent = File.ReadAllText(path);
        characters = JsonConvert.DeserializeObject<List<Character>>(fileContent);
        //  Character ch = JsonConvert.DeserializeObject<Character>(fileContent);
        //   Debug.Log(ch.Name);
        CreateCharacters();
    }

    void CreateCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            string name = characters[i].Name;
            Debug.Log(name);
            GameObject g = (GameObject)Instantiate(Resources.Load(name));
            g.transform.position = characters[i].Pos;
            g.SetActive(true);
        }
    }
}

[System.Serializable]
public class Character
{
    public string Name;
    public Vector3 Pos;

}