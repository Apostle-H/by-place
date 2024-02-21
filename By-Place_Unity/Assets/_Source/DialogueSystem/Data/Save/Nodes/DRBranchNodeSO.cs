using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRBranchNodeSO : DRNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
    }
}