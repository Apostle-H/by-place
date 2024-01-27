using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DRGroupSO> Groups { get; private set; } = new();
        [field: SerializeField] public List<DRNodeSO> Nodes { get; private set; } = new();
    }
}