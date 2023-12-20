using System;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class DSNodeChoiceSave
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public GUID NextNodeGuid { get; set; } = new();
    }
}