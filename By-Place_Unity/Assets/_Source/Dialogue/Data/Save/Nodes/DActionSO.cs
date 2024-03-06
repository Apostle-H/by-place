using ActionSystem.Data;
using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public class DActionSO : DNodeSO
    {
        [field: SerializeField] public ActionSO ActionSO { get; set; }
    }
}