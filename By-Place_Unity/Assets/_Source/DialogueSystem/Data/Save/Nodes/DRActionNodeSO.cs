using ActionSystem.Data;
using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DRActionNodeSO : DRNodeSO
    {
        [field: SerializeField] public ActionSO ActionSO { get; set; }
    }
}