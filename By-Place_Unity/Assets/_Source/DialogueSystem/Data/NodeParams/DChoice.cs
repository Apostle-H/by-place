using System;
using UnityEngine;

namespace DialogueSystem.Data.NodeParams
{
    [Serializable]
    public class DChoice
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DVariableSO CheckVariableSO { get; set; }
        [field: SerializeField] public bool ExpectedValue { get; set; }
    }
}