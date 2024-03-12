using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public abstract class DObjectNodeSO<T> : DNodeSO where T : Object
    {
        [field: SerializeField] public T Value { get; set; }
    }
}