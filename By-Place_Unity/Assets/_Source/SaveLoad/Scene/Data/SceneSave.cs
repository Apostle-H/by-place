using SaveLoad.Json.Data;
using UnityEngine;

namespace SaveLoad.Scene.Data
{
    public class SceneSave
    {
        public bool Active { get; set; }
        public JsonVector4 Position { get; set; }
        public JsonVector4 Rotation { get; set; }

        public SceneSave(bool active, JsonVector4 position, JsonVector4 rotation)
        {
            Active = active;
            Position = position;
            Rotation = rotation;
        }
    }
}