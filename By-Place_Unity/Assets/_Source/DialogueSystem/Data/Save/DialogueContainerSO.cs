using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DialogueGroupSO> GroupsSOs { get; private set; } = new();
        [field: SerializeField] public List<DialogueNodeSO> UngroupedDialogues { get; private set; } = new();
    }
}