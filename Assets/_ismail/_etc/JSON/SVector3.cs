using System;
using UnityEngine;
namespace SerializableTypes
{

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
}
