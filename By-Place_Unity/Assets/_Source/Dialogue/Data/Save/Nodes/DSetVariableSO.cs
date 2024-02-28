using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DSetVariableSO : DNodeSO
    {
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public bool SetValue { get; set; }
    }
}