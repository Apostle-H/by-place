using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue.Data.NodeParams;
using Dialogue.Data.Save;
using Dialogue.Data.Save.Nodes;
using DialogueSystem.Elements;
using DialogueSystem.Elements.Nodes;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Utils.Services;

namespace DialogueSystem.Utils
{
    public static class DGSaveLoad
    {
        public static readonly string DS_CONTAINER_SAVE_SO_PATH = "Assets/_Presentation/Dialogues";

        public static DGEditorWindow EditorWindow { get; set; }

        public static void Save(DGraphView graphView, string graphName)
        {
            var containerPath = $"{DS_CONTAINER_SAVE_SO_PATH}/{graphName}";
            graphView.GetElements(out var nodes, out var groups);

            var rContainerSO = AssetsService.CreateAsset<DContainerSO>(containerPath, graphName);
            rContainerSO.Groups.Clear();
            rContainerSO.Nodes.Clear();
            
            MoveNodes(nodes, groups, rContainerSO, graphView.movedNodesGuids);
            DeleteNodes(graphView.deletedNodesRuntimeIds);
            RenameGroups(groups, graphView.renamedGroupsGuids);
            DeleteGroups(graphView.deletedGroupGuids);
            
            SaveGroups(rContainerSO, containerPath, groups);
            SaveUngrouped(rContainerSO, nodes);
            
            rContainerSO.FileName = graphName;
            AssetsService.SaveAsset(rContainerSO);
            
            graphView.ClearChanges();
        }
        
        public static void Load(DGraphView graphView, string graphName)
        {
            var containerSO = AssetsService.LoadAsset<DContainerSO>($"{DS_CONTAINER_SAVE_SO_PATH}/{graphName}", graphName);
            
            if (containerSO == null)
            {
                EditorUtility.DisplayDialog("Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"{DS_CONTAINER_SAVE_SO_PATH}/{graphName}\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            var loadedNodes = new Dictionary<int, DNode>();
            LoadNodes(containerSO, graphView, ref loadedNodes);
            LoadGroups(containerSO, graphView, loadedNodes);
            
            LoadNodesConnections(graphView, loadedNodes);
            
            EditorWindow.UpdateFileName(graphName);
        }
        
        private static void RenameGroups(List<DGroup> groups, HashSet<int> renamedGroupsGuids)
        {
            foreach (var guid in renamedGroupsGuids)
            {
                var group = groups.First(group => group.Guid == guid);
                AssetsService.RenameAsset(group.RuntimeAssetId, group.title);
            }
            
            renamedGroupsGuids.Clear();
        }
        
        private static void DeleteGroups(HashSet<int> deletedGroupsRuntimeIds)
        {
            foreach (var id in deletedGroupsRuntimeIds)
                AssetsService.DeleteAsset(id);
            
            deletedGroupsRuntimeIds.Clear();
        }

        private static void MoveNodes(List<DNode> nodes, List<DGroup> groups, DContainerSO container, HashSet<int> movedNodesGuids)
        {
            foreach (var guid in movedNodesGuids)
            {
                var node = nodes.First(node => node.Guid == guid);
                var group = groups.FirstOrDefault(group => group.Guid == node.GroupGuid);
                AssetsService.MoveAsset(node.RuntimeAssetId, group?.RuntimeAssetId ?? container.GetInstanceID());
            }
            
            movedNodesGuids.Clear();
        }
        
        private static void DeleteNodes(HashSet<int> deletedNodesRuntimeIds)
        {
            foreach (var id in deletedNodesRuntimeIds)
                AssetsService.DeleteAsset(id);
            
            deletedNodesRuntimeIds.Clear();
        }
        
        private static void SaveGroups(DContainerSO rContainerSO, string containerPath, List<DGroup> groups)
        {
            foreach (var group in groups)
            {
                var groupSO = AssetsService.CreateAsset<DGroupSO>(containerPath, group.title);
                groupSO.Guid = group.Guid;
                groupSO.Name = group.title;
                groupSO.Owner = rContainerSO;
                
                groupSO.NodesSOs.Clear();
                foreach (var kvp in group.Nodes)
                {
                    var node = kvp.Value;

                    var nodeSO = SaveNode(rContainerSO, node, groupSO);
                    groupSO.NodesSOs.Add(nodeSO);
                    
                    if (!nodeSO.IsStartingNode)
                        continue;
                    groupSO.StartingNodeGuid = nodeSO.Guid;
                }

                rContainerSO.Groups.Add(groupSO);

                AssetsService.SaveAsset(groupSO);
                group.RuntimeAssetId = groupSO.GetInstanceID();
            }
        }

        private static void SaveUngrouped(DContainerSO rContainerSO, List<DNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.GroupGuid != -1)
                    continue;

                SaveNode(rContainerSO, node, rContainerSO);
            }
        }

        private static DNodeSO SaveNode(DContainerSO rContainerSO, DNode node, ScriptableObject parent)
        {
            DNodeSO nodeSO;
            if (node is DDialogueNode dialogueNode)
            {
                var dialogueNodeSO = AssetsService.CreateSubAsset<DDialogueSO>(parent, dialogueNode.Guid.ToString());
                dialogueNodeSO.SpeakerSO = dialogueNode.SpeakerSO;
                dialogueNodeSO.Texts = dialogueNode.Texts.ToList();
                dialogueNodeSO.Choices = dialogueNode.Choices.ToList();
                
                nodeSO = dialogueNodeSO;
            }
            else if (node is DActionNode actionNode)
            {
                var actionNodeSO = AssetsService.CreateSubAsset<DActionSO>(parent, actionNode.Guid.ToString());
                actionNodeSO.ActionSO = actionNode.Value;
                
                nodeSO = actionNodeSO;
            }
            else if (node is DAnimationNode animationNode)
            {
                var animationNodeSO = AssetsService.CreateSubAsset<DAnimationSO>(parent, animationNode.Guid.ToString());
                animationNodeSO.IdentitySO = animationNode.IdentitySO;
                animationNodeSO.Animation = animationNode.Animation;

                nodeSO = animationNodeSO;
            }
            else if (node is DSoundNode soundNode)
            {
                var soundNodeSO = AssetsService.CreateSubAsset<DSoundSO>(parent, soundNode.Guid.ToString());
                soundNodeSO.Value = soundNode.Value;

                nodeSO = soundNodeSO;
            }
            else if (node is DSetVariableNode setVariableNode)
            {
                var setVariableNodeSO = AssetsService.CreateSubAsset<DSetVariableSO>(parent, setVariableNode.Guid.ToString());
                setVariableNodeSO.VariableSO = setVariableNode.VariableSO;
                setVariableNodeSO.SetValue = setVariableNode.SetValue;

                nodeSO = setVariableNodeSO;
            }
            else if (node is DBranchNode branchNode)
            {
                var branchNodeSO = AssetsService.CreateSubAsset<DBranchSO>(parent, branchNode.Guid.ToString());
                branchNodeSO.VariableSO = branchNode.VariableSO;

                nodeSO = branchNodeSO;
            }
            else
                throw new ArgumentException($"Unexpected node type {node.GetType()} in the graph");
            
            nodeSO.Guid = node.Guid;
            nodeSO.IsStartingNode = node.IsStartingNode();
            nodeSO.OutputData = node.NextGuids.ToList();
            
            nodeSO.Position = node.GetPosition().position;

            rContainerSO.Nodes.Add(nodeSO);
            AssetsService.SaveAsset(nodeSO);
            node.RuntimeAssetId = nodeSO.GetInstanceID();
            
            return nodeSO;
        }
        
        private static void LoadGroups(DContainerSO containerSO, DGraphView graphView, Dictionary<int, DNode> loadedNodes)
        {
            foreach (var groupSave in containerSO.Groups)
            {
                var group = new DGroup(groupSave.Name, Vector2.zero, groupSave.Guid, groupSave.GetInstanceID());
                graphView.AddGroup(group);
                
                foreach (var nodeSO in groupSave.NodesSOs)
                {
                    var loadedNode = loadedNodes[nodeSO.Guid];
                    
                    loadedNode.GroupGuid = group.Guid;
                    group.AddElement(loadedNode);
                }
            }
        }
        
        private static void LoadNodes(DContainerSO containerSO, DGraphView graphView, ref Dictionary<int, DNode> loadedNodes)
        {
            foreach (var nodeSave in containerSO.Nodes)
            {
                var node = LoadNode(nodeSave, graphView);
                
                graphView.AddNode(node);
                loadedNodes.Add(node.Guid, node);
            }
        }

        private static DNode LoadNode(DNodeSO nodeSO, DGraphView graphView)
        {
            DNode node;
            switch (nodeSO)
            {
                case DDialogueSO dialogueNodeSO:
                    var dialogueNode = new DDialogueNode(nodeSO.Position, nodeSO.Guid)
                    {
                        SpeakerSO = dialogueNodeSO.SpeakerSO,
                        Choices = dialogueNodeSO.Choices.ToList(),
                        Texts = dialogueNodeSO.Texts.ToList()
                    };

                    node = dialogueNode;
                    break;
                case DActionSO actionNodeSO:
                    var actionNode = new DActionNode(nodeSO.Position, nodeSO.Guid)
                    {
                        Value = actionNodeSO.ActionSO
                    };

                    node = actionNode;
                    break;
                case DAnimationSO animationNodeSO:
                    var animationNode = new DAnimationNode(nodeSO.Position, nodeSO.Guid)
                    {
                        IdentitySO = animationNodeSO.IdentitySO,
                        Animation = animationNodeSO.Animation
                    };

                    node = animationNode;
                    break;
                case DSoundSO soundNodeSO:
                    var soundNode = new DSoundNode(nodeSO.Position, nodeSO.Guid)
                    {
                        Value = soundNodeSO.Value
                    };

                    node = soundNode;
                    break;
                case DSetVariableSO setVariableNodeSO:
                    var setVariableNode = new DSetVariableNode(nodeSO.Position, nodeSO.Guid)
                    {
                        VariableSO = setVariableNodeSO.VariableSO,
                        SetValue = setVariableNodeSO.SetValue
                    };

                    node = setVariableNode;
                    break;
                case DBranchSO branchNodeSO:
                    var branchNode = new DBranchNode(nodeSO.Position, nodeSO.Guid)
                    {
                        VariableSO = branchNodeSO.VariableSO
                    };

                    node = branchNode;
                    break;
                default:
                    throw new ArgumentException($"Unexpected node type in the save");
            }

            node.NextGuids = nodeSO.OutputData.ToList();
            node.RuntimeAssetId = nodeSO.GetInstanceID();

            return node;
        }
        
        private static void LoadNodesConnections(DGraphView graphView, Dictionary<int, DNode> loadedNodes)
        {
            foreach (KeyValuePair<int, DNode> loadedNode in loadedNodes)
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