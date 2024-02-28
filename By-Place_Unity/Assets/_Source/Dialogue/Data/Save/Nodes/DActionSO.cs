using ActionSystem.Data;
using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DActionSO : DNodeSO
    {
        [field: SerializeField] public ActionSO ActionSO { get; set; }
    }
}