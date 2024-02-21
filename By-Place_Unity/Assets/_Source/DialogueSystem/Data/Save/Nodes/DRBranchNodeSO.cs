using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DRBranchNodeSO : DRNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
    }
}