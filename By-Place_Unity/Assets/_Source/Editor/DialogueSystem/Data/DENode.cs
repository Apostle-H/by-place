using System;
using System.Collections.Generic;
using DialogueSystem.ActionSystem.Data;
using DialogueSystem.Data.Save;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DENode
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public int GroupGuid { get; set; } = -1;
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public List<DOutputData> NextGuids { get; set; }
        
        [field: SerializeField] public DNodeType NodeType { get; set; }
        
        [field: Header("DialogueNode")]
        [field: SerializeField] public DSpeakerSO SpeakerSO { get; set; } 
        [field: SerializeField] public List<DChoice> Choices { get; set; }
        [field: SerializeField] public List<DText> Texts { get; set; }
        
        [field: Header("Action")]
        [field: SerializeField] public ActionSO ActionSO { get; set; }
        
        [field: Header("Variable")]
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public bool SetValue { get; set; }
    }
}