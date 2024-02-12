using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRSetVariableNodeSO : DRNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public bool SetValue { get; set; }
    }
}