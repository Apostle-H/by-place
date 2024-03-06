using System.Collections.Generic;
using Dialogue.Data.NodeParams;
using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public class DDialogueSO : DNodeSO
    {
        [field: SerializeField] public DSpeakerSO SpeakerSO { get; set; }
        [field: SerializeField] public List<DChoice> Choices { get; set; }
        [field: SerializeField] public List<DText> Texts { get; set; }
    }
}