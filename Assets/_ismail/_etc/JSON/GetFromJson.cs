using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRest;
using System.IO;

public static class GetFromJson<T>
{
    public static T[] GetArray(string path)
    {
        var fileContent = File.ReadAllText(path);
        T[] array = JsonHelper.FromJson<T>(fileContent);
        return array;
    }
}
