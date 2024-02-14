using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRDialogueNodeSO : DRNodeSO
    {
        [field: SerializeField] public DSpeakerSO SpeakerSO { get; set; }
        [field: SerializeField] public List<DChoice> Choices { get; set; }
        [field: SerializeField] public List<DText> Texts { get; set; }
    }
}