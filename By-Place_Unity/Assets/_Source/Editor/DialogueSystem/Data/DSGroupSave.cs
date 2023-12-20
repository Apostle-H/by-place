using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class DSGroupSave
    {
        [field: SerializeField] public GUID Guid { get; set; } = new();
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}