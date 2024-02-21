using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Elements;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEngine;
using Utils.Services;

namespace DialogueSystem.Utils
{
    public static class DGSaveLoad
    {
        private static readonly string DS_GRAPH_SAVE_DATA_SO_PATH = "Assets/_Source/Editor/DialogueSystem/Graphs";
        private static readonly string DS_CONTAINER_SAVE_SO_PATH = "Assets/_Presentation/Dialogues";

        public static DGEditorWindow EditorWindow { get; set; }

        public static void Save(DSGraphView graphView, string graphName)
        {
            var containerPath = $"{DS_CONTAINER_SAVE_SO_PATH}/{graphName}";
            graphView.GetElements(out var nodes, out var groups);

            var eContainerSO = AssetsService.CreateAsset<DEContainerSO>(DS_GRAPH_SAVE_DATA_SO_PATH, $"{graphName}Graph");
            eContainerSO.Groups.Clear();
            eContainerSO.Nodes.Clear();
            var rContainerSO = AssetsService.CreateAsset<DRContainerSO>(containerPath, graphName);
            rContainerSO.Groups.Clear();
            rContainerSO.Nodes.Clear();
            
            MoveNodes(nodes, groups, rContainerSO, graphView.movedNodesGuids);
            DeleteNodes(graphView.deletedNodesRuntimeIds);
            RenameGroups(groups, graphView.renamedGroupsGuids);
            DeleteGroups(graphView.deletedGroupGuids);
            
            SaveGroupsRContainer(rContainerSO, containerPath, groups);
            SaveUngroupedRContainer(rContainerSO, nodes);
            eContainerSO.Save(groups, nodes);
            
            eContainerSO.FileName = graphName;
            AssetsService.SaveAsset(eContainerSO);
            rContainerSO.FileName = graphName;
            AssetsService.SaveAsset(rContainerSO);
        }
        
        public static void Load(DSGraphView graphView, string graphName)
        {
            var graphSave = AssetsService.LoadAsset<DEContainerSO>(DS_GRAPH_SAVE_DATA_SO_PATH, graphName);

            if (graphSave == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"Assets/Editor/DialogueSystem/Graphs/{graphName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            graphSave.Load(graphView);
            EditorWindow.UpdateFileName(graphName[0..^5]);
        }
        
        private static void RenameGroups(List<DGGroup> groups, HashSet<int> renamedGroupsGuids)
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

        private static void MoveNodes(List<DGNode> nodes, List<DGGroup> groups, DRContainerSO container, HashSet<int> movedNodesGuids)
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
        
        private static void SaveGroupsRContainer(DRContainerSO rContainerSO, string containerPath, List<DGGroup> groups)
        {
            foreach (var group in groups)
            {
                var groupSO = AssetsService.CreateAsset<DRGroupSO>(containerPath, group.title);
                groupSO.Guid = group.Guid;
                groupSO.Name = group.title;
                groupSO.Owner = rContainerSO;
                
                foreach (var kvp in group.Nodes)
                {
                    var node = kvp.Value;

                    var nodeSO = SaveNodeRContainer(rContainerSO, node, groupSO);
                    
                    if (!nodeSO.IsStartingNode)
                        continue;
                    groupSO.StartingNodeGuid = nodeSO.Guid;
                }

                rContainerSO.Groups.Add(groupSO);

                AssetsService.SaveAsset(groupSO);
                group.RuntimeAssetId = groupSO.GetInstanceID();
            }
        }

        private static void SaveUngroupedRContainer(DRContainerSO rContainerSO, List<DGNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.GroupGuid != -1)
                    continue;

                SaveNodeRContainer(rContainerSO, node, rContainerSO);
            }
        }

        private static DRNodeSO SaveNodeRContainer(DRContainerSO rContainerSO, DGNode node, ScriptableObject parent)
        {
            DRNodeSO nodeSO;
            if (node is DGDialogueNode dialogueNode)
            {
                var dialogueNodeSO = AssetsService.CreateSubAsset<DRDialogueNodeSO>(parent, dialogueNode.Guid.ToString());
                dialogueNodeSO.SpeakerSO = dialogueNode.SpeakerSO;
                dialogueNodeSO.Texts = dialogueNode.Texts.ToList();
                dialogueNodeSO.Choices = dialogueNode.Choices.ToList();
                
                nodeSO = dialogueNodeSO;
            }
            else if (node is DGActionNode actionNode)
            {
                var actionNodeSO = AssetsService.CreateSubAsset<DRActionNodeSO>(parent, actionNode.Guid.ToString());
                actionNodeSO.ActionSO = actionNode.ActionSO;
                
                nodeSO = actionNodeSO;
            }
            else if (node is DGSetVariableNode setVariableNode)
            {
                var setVariableNodeSO = AssetsService.CreateSubAsset<DRSetVariableNodeSO>(parent, setVariableNode.Guid.ToString());
                setVariableNodeSO.VariableSO = setVariableNode.VariableSO;
                setVariableNodeSO.SetValue = setVariableNode.SetValue;

                nodeSO = setVariableNodeSO;
            }
            else if (node is DGBranchNode branchNode)
            {
                var branchNodeSO = AssetsService.CreateSubAsset<DRBranchNodeSO>(parent, branchNode.Guid.ToString());
                branchNodeSO.VariableSO = branchNode.VariableSO;

                nodeSO = branchNodeSO;
            }
            else
                throw new ArgumentException($"Unexpected node type {node.GetType()} in the graph");
            
            nodeSO.Guid = node.Guid;
            nodeSO.IsStartingNode = node.IsStartingNode();
            nodeSO.NextGuids = node.NextGuids.ToList();

            rContainerSO.Nodes.Add(nodeSO);
            AssetsService.SaveAsset(nodeSO);
            node.RuntimeAssetId = nodeSO.GetInstanceID();
            
            return nodeSO;
        }
    }
}