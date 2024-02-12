using DialogueSystem.ActionSystem.Data;
using UnityEngine;

namespace DialogueSystem.Data.Save
{
    public class DRActionNodeSO : DRNodeSO
    {
        [field: SerializeField] public ActionSO ActionSO { get; set; }
    }
}