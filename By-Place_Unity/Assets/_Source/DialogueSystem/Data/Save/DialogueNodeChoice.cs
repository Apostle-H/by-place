using System;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem.Scripts.Data
{
    [Serializable]
    public class DialogueNodeChoice
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DialogueNodeSO NextNode { get; set; }
    }
}