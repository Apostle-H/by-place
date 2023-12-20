using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Scripts.Data;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DialogueNodeSO : ScriptableObject
    {
        [field: SerializeField] public GUID Guid { get; set; }
        [field: SerializeField] public DialogueSpeakerSO SpeakerSO { get; set; }
        [field: SerializeField, TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DialogueNodeChoice> Choices { get; set; }
        [field: SerializeField] public DialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
    }
}