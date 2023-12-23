using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DialogueNodeSO : ScriptableObject
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public DialogueSpeakerSO SpeakerSO { get; set; }
        [field: SerializeField, TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DialogueNodeChoice> Choices { get; set; }
        [field: SerializeField] public DialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
    }
}