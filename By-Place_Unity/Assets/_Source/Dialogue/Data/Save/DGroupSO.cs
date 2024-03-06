using System.Collections.Generic;
using Dialogue.Data.Save.Nodes;
using UnityEngine;

namespace Dialogue.Data.Save
{
    public class DGroupSO : ScriptableObject
    {
        [field: SerializeField] public DContainerSO Owner { get; set; }
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public int StartingNodeGuid { get; set; }
        [field: SerializeField] public List<DNodeSO> NodesSOs { get; private set; } = new();
    }
}