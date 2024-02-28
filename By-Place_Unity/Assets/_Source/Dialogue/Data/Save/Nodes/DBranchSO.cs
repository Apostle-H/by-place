using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DBranchSO : DNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
    }
}