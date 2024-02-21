using System;
using UnityEngine;

namespace DialogueSystem.Data.NodeParams
{
    [Serializable]
    public class DText
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
    }
}