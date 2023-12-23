using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DSNodeSave
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public DialogueSpeakerSO SpeakerSO { get; set; } 
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<DSNodeChoiceSave> Choices { get; set; }
        [field: SerializeField] public int GroupGuid { get; set; } = -1;
        [field: SerializeField] public DialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}