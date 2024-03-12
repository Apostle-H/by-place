using ActionSystem.Data;
using UnityEngine;

namespace DialogueSystem.Elements.Nodes
{
    public class DActionNode : DObjectNode<ActionSO>
    {
        public DActionNode(Vector2 position, int guid = -1) : base("Action", position, guid) { }

        public override DNode NewAt(Vector2 position) => new DActionNode(position);
    }
}