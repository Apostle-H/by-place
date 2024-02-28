using System.Collections.Generic;
using DialogueSystem.Data.Save.Nodes;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DGroupSO : ScriptableObject
    {
        [field: SerializeField] public DContainerSO Owner { get; set; }
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public int StartingNodeGuid { get; set; }
        [field: SerializeField] public List<DNodeSO> NodesSOs { get; private set; }
    }
}