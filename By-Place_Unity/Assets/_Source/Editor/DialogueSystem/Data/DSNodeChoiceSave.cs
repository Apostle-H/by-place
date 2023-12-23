using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DSNodeChoiceSave
    {
        [field: SerializeField] public int NextNodeGuid { get; set; } = -1;
        [field: SerializeField] public string Text { get; set; }
    }
}