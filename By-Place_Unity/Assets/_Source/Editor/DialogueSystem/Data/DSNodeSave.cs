using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class DSNodeSave
    {
        [field: SerializeField] public GUID Guid { get; set; } = new();
        [field: SerializeField] public DialogueSpeakerSO SpeakerSO { get; set; } 
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<DSNodeChoiceSave> Choices { get; set; }
        [field: SerializeField] public GUID GroupGuid { get; set; } = new();
        [field: SerializeField] public DialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}