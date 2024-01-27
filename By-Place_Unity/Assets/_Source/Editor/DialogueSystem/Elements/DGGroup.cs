using System.Collections.Generic;
using DialogueSystem.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DGGroup : Group
    {
        public int Guid { get; set; } = -1;
        public Dictionary<int, DGNode> Nodes { get; private set; } = new();

        public string PreviousName { get; set; }

        public DGGroup(string groupTitle, Vector2 position)
        {
            Guid = IDGenerator.NewId();

            title = groupTitle;
            PreviousName = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }

        public void AddNode(DGNode node)
        {
            if (Nodes.ContainsKey(node.Guid))
                return;

            Nodes.Add(node.Guid, node);
            Nodes[node.Guid].GroupGuid = Guid;
        }

        public void RemoveNode(DGNode node)
        {
            if (!Nodes.ContainsKey(node.Guid))
                return;

            Nodes[node.Guid].GroupGuid = -1;
            Nodes.Remove(node.Guid);
        }
    }
}