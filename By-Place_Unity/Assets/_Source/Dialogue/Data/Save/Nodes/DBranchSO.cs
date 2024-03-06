using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public class DBranchSO : DNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
    }
}