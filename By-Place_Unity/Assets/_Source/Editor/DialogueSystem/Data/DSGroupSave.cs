using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DSGroupSave
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}