using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DRSetVariableNodeSO : DRNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public bool SetValue { get; set; }
    }
}