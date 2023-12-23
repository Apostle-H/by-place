using System;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    [Serializable]
    public class DialogueNodeChoice
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DialogueNodeSO NextNode { get; set; }
    }
}