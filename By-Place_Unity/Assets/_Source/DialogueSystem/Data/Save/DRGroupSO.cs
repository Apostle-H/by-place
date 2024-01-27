using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRGroupSO : ScriptableObject
    {
        [field: SerializeField] public DRContainerSO Owner { get; set; }
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public int StartingNodeGuid { get; set; }
    }
}