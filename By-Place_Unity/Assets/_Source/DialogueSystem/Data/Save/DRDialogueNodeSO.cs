using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRDialogueNodeSO : DRNodeSO
    {
        [field: SerializeField] public DSpeakerSO SpeakerSO { get; set; }
        [field: SerializeField, TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<string> ChoicesText { get; set; }
    }
}