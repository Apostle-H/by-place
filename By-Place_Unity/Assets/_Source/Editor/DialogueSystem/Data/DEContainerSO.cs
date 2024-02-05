using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data.Save;
using DialogueSystem.Elements;
using DialogueSystem.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Data
{
    public class DEContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DEGroup> Groups { get; private set; } = new();
        [field: SerializeField] public List<DENode> Nodes { get; private set; } = new();

        public void Save(List<DGGroup> groups, List<DGNode> nodes)
        {
            SaveGroups(groups);
            SaveNodes(nodes);
        }
        
        private void SaveGroups(List<DGGroup> groups)
        {
            foreach (var group in groups)
            {
                Groups.Add(new DEGroup()
                {
                    Guid = group.Guid,
                    Name = group.title,
                    Position = group.GetPosition().position
                });
            }
        }
        
        private void SaveNodes(List<DGNode> nodes)
        {
            foreach (var node in nodes)
            {
                var nodeSave = new DENode()
                {
                    Guid = node.Guid,
                    GroupGuid = node.GroupGuid,
                    Position = node.GetPosition().position,
                    NextGuids = node.NextGuids.ToList()
                };
            
                if (node is DGDialogueNode dialogueNode)
                {
                    nodeSave.NodeType = DNodeType.DIALOGUE;
                    nodeSave.SpeakerSO = dialogueNode.SpeakerSO;
                    nodeSave.Choices = dialogueNode.Choices.ToList();
                    nodeSave.Texts = dialogueNode.Texts.ToList();
                
                    Nodes.Add(nodeSave);
                }
                else if (node is DGActionNode actionNode)
                {
                    nodeSave.NodeType = DNodeType.ACTION;
                    nodeSave.TargetSO = actionNode.TargetSO;
                
                    Nodes.Add(nodeSave);
                }
            }
        }
        
        public void Load(DSGraphView graphView)
        {
            var loadedGroups = new Dictionary<int, DGGroup>();
            LoadGroups(graphView, ref loadedGroups);
            
            var loadedNodes = new Dictionary<int, DGNode>();
            LoadNodes(graphView, loadedGroups, ref loadedNodes);
            
            LoadNodesConnections(graphView, loadedNodes);
        }

        private void LoadGroups(DSGraphView graphView, ref Dictionary<int, DGGroup> loadedGroups)
        {
            foreach (var groupSave in Groups)
            {
                var group = graphView.CreateGroup(groupSave.Name, groupSave.Position, groupSave.Guid);
                group.title = groupSave.Name;
                
                loadedGroups.Add(group.Guid, group);
            }
        }
        
        private void LoadNodes(DSGraphView graphView, Dictionary<int, DGGroup> loadedGroups, 
            ref Dictionary<int, DGNode> loadedNodes)
        {
            foreach (var nodeSave in Nodes)
            {
                DGNode node;
                switch (nodeSave.NodeType)
                {
                    case DNodeType.DIALOGUE:
                    {
                        var dialogueNode = graphView.CreateNode<DGDialogueNode>(nodeSave.Position, nodeSave.Guid, false);
                        dialogueNode.SpeakerSO = nodeSave.SpeakerSO;
                        dialogueNode.Choices = nodeSave.Choices.ToList();
                        dialogueNode.Texts = nodeSave.Texts.ToList();

                        node = dialogueNode;
                        break;
                    }
                    case DNodeType.ACTION:
                    {
                        var actionNode = graphView.CreateNode<DGActionNode>(nodeSave.Position, nodeSave.Guid, false);
                        actionNode.TargetSO = nodeSave.TargetSO;

                        node = actionNode;
                        break;
                    }
                    default:
                        throw new ArgumentException($"Unexpected node type {nodeSave.GetType()} in the save");
                }

                node.NextGuids = nodeSave.NextGuids.ToList();
                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.Guid, node);

                if (nodeSave.GroupGuid == -1)
                    continue;

                var group = loadedGroups[nodeSave.GroupGuid];
                node.GroupGuid = group.Guid;
                
                group.AddElement(node);
            }
        }
        
        private void LoadNodesConnections(DSGraphView graphView, Dictionary<int, DGNode> loadedNodes)
        {
            foreach (KeyValuePair<int, DGNode> loadedNode in loadedNodes)
            {
                foreach (var visualElement in loadedNode.Value.outputContainer.Children())
                {
                    var choicePort = (Port)visualElement;
                    var outputData = (DOutputData)choicePort.userData;

                    if (outputData.NextGuid == -1)
                        continue;

                    var nextNode = loadedNodes[outputData.NextGuid];
                    var nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                    var edge = choicePort.ConnectTo(nextNodeInputPort);
                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }
    }
}