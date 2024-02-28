using System.Collections.Generic;
using DialogueSystem.Elements.Nodes;
using DialogueSystem.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DGroup : Group
    {
        public int Guid { get; private set; }
        public Dictionary<int, DNode> Nodes { get; private set; } = new();

        public int RuntimeAssetId { get; set; }
        
        public DGroup(string groupTitle, Vector2 position, int guid = -1, int runtimeAssetId = 0)
        {
            Guid = guid == -1 ? IDGenerator.NewId() : guid;
            RuntimeAssetId = runtimeAssetId;
            
            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }

        public void AddNode(DNode node)
        {
            if (Nodes.ContainsKey(node.Guid))
                return;

            Nodes.Add(node.Guid, node);
            Nodes[node.Guid].GroupGuid = Guid;
        }

        public void RemoveNode(DNode node)
        {
            if (!Nodes.ContainsKey(node.Guid))
                return;

            Nodes[node.Guid].GroupGuid = -1;
            Nodes.Remove(node.Guid);
        }
    }
}