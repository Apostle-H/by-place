using UnityEditor.UIElements;
using UnityEngine;

namespace DialogueSystem.Elements.Nodes
{
    public class DSoundNode : DObjectNode<AudioClip>
    {
        public DSoundNode(Vector2 position, int guid = -1) : base("Sound", position, guid) { }
       
        public override DNode NewAt(Vector2 position) => new DSoundNode(position);
    }
}