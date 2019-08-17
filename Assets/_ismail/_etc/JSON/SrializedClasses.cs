using UnityEngine;
using System;

[Serializable]
public struct Word
{
    public string word;
    public Word(string _str)
    {
        this.word = _str;
    }
    public static implicit operator string(Word s) => s.word;
}

[System.Serializable]
public struct CharIntro
{
    public Vector3 Pos;
    public string Action;
    public string Talk0;
}

[Serializable]
public struct SVector3
{
    public float x;
    public float y;
    public float z;

    public SVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator Vector3(SVector3 s) => new Vector3(s.x, s.y, s.z);
}