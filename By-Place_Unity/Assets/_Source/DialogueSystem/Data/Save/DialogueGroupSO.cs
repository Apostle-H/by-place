using DialogueSystem.Utilities;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DialogueGroupSO : ScriptableObject
    {
        [field: SerializeField] public GUID Guid { get; set; }
        [field: SerializeField] public string Name { get; set; }
        
        [field: SerializeField] public DialogueNodeSO StartingNode { get; set; }
        [field: SerializeField] public SerializableDictionary<GUID, DialogueNodeSO> NodesSOs { get; set; } = new();
    }
}