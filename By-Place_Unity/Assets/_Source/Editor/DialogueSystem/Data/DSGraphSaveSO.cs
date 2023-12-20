using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DSGraphSaveSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DSGroupSave> Groups { get; private set; } = new();
        [field: SerializeField] public List<DSNodeSave> Nodes { get; private set; } = new();
    }
}