using System.Collections.Generic;
using DialogueSystem.Data.NodeParams;
using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DNodeSO : ScriptableObject
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public bool IsStartingNode { get; set; }
        [field: SerializeField] public List<DOutputData> OutputData { get; set; }

#if UNITY_EDITOR
        [field: SerializeField] public Vector2 Position { get; set; }
#endif
    }
}