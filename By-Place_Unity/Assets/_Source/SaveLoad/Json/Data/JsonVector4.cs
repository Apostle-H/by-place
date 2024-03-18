using UnityEngine;

namespace SaveLoad.Json.Data
{
    public class JsonVector4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public JsonVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector3 GetUnityVector() => new Vector3(X, Y, Z);

        public Quaternion GetUnityQuaternion() => new Quaternion(X, Y, Z, W);
    }
}