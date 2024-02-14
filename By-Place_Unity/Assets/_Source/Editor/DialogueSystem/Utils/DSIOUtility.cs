using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Elements;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Utils
{
    public static class DSIOUtility
    {
        private static readonly string DS_GRAPH_SAVE_DATA_SO_PATH = "Assets/_Source/Editor/DialogueSystem/Graphs";
        private static readonly string DS_CONTAINER_SAVE_SO_PATH = "Assets/_Presentation/Dialogues";

        public static void Save(DSGraphView graphView, string graphName)
        {
            var containerPath = $"{DS_CONTAINER_SAVE_SO_PATH}/{graphName}";
            GetElementsFromGraphView(graphView, out var nodes, out var groups);

            var eContainerSO = CreateAsset<DEContainerSO>(DS_GRAPH_SAVE_DATA_SO_PATH, $"{graphName}Graph");
            eContainerSO.Groups.Clear();
            eContainerSO.Nodes.Clear();
            var rContainerSO = CreateAsset<DRContainerSO>(containerPath, graphName);
            rContainerSO.Groups.Clear();
            rContainerSO.Nodes.Clear();
            
            RenameGroups(containerPath, groups.ToDictionary(group => group.Guid, group => group.title),
                graphView.renamedGroups);
            DeleteGroups(containerPath, graphView.deletedGroupsNames);
            DeleteNodes(containerPath, graphView.deletedNodesGuids);
            
            eContainerSO.Save(groups, nodes);
            SaveGroupsRContainer(rContainerSO, containerPath, groups);
            SaveUngroupedRContainer(rContainerSO, containerPath, nodes);
            
            eContainerSO.FileName = graphName;
            SaveAsset(eContainerSO);
            rContainerSO.FileName = graphName;
            SaveAsset(rContainerSO);
        }
        
        public static void Load(DSGraphView graphView, string graphName)
        {
            var graphSave = LoadAsset<DEContainerSO>(DS_GRAPH_SAVE_DATA_SO_PATH, graphName);

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

            DSEditorWindow.UpdateFileName(graphSave.FileName);

            graphSave.Load(graphView);
        }
        
        private static void RenameGroups(string containerPath, Dictionary<int, string> groupsNames,
            Dictionary<int, string> renamedGroups)
        {
            foreach (var kvp in renamedGroups)
            {
                var groupFolderPath = $"{containerPath}/{kvp.Value}";
                if (!AssetDatabase.IsValidFolder(groupFolderPath + "/"))
                    continue;

                var groupName = groupsNames.First(group => group.Key == kvp.Key).Value;
                AssetDatabase.RenameAsset($"{groupFolderPath}/{kvp.Value}.asset", $"{groupName}");
                AssetDatabase.RenameAsset(groupFolderPath, groupName);
            }
        }
        
        private static void DeleteGroups(string containerPath, List<string> deletedGroupsNames)
        {
            foreach (var deletedGroupName in deletedGroupsNames)
            {
                var groupFolderPath = $"{containerPath}/{deletedGroupName}";
                if (AssetDatabase.IsValidFolder(groupFolderPath))
                    AssetDatabase.DeleteAsset(groupFolderPath);
            }
        }
        
        private static void DeleteNodes(string containerPath, List<int> deletedNodesGuid)
        {
            foreach (var deletedNodeGuid in deletedNodesGuid)
            {
                var node = AssetDatabase
                    .FindAssets(deletedNodeGuid.ToString(), new[] { containerPath });
                if (node.Length > 0)
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(node[0]));
            }
        }
        
        private static void SaveGroupsRContainer(DRContainerSO rContainerSO, string containerPath, 
            List<DGGroup> groups)
        {
            foreach (var group in groups)
            {
                var groupSO = CreateAsset<DRGroupSO>($"{containerPath}/{group.title}", group.title);
                groupSO.Guid = group.Guid;
                groupSO.Name = group.title;
                groupSO.Owner = rContainerSO;
                
                foreach (var kvp in group.Nodes)
                {
                    var node = kvp.Value;

                    var nodeSO = SaveNodeRContainer(rContainerSO, node, $"{containerPath}/{group.title}/Nodes");
                    
                    if (!nodeSO.IsStartingNode)
                        continue;
                    groupSO.StartingNodeGuid = nodeSO.Guid;
                }

                rContainerSO.Groups.Add(groupSO);

                SaveAsset(groupSO);
            }
        }

        private static void SaveUngroupedRContainer(DRContainerSO rContainerSO, string containerPath,
            List<DGNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.GroupGuid != -1)
                    continue;

                SaveNodeRContainer(rContainerSO, node, $"{containerPath}/Ungrouped");
            }
        }

        private static DRNodeSO SaveNodeRContainer(DRContainerSO rContainerSO, DGNode node, string savePath)
        {
            DRNodeSO nodeSO;
            if (node is DGDialogueNode dialogueNode)
            {
                var dialogueNodeSO = CreateAsset<DRDialogueNodeSO>(savePath, dialogueNode.Guid.ToString());
                dialogueNodeSO.SpeakerSO = dialogueNode.SpeakerSO;
                dialogueNodeSO.Texts = dialogueNode.Texts.ToList();
                dialogueNodeSO.Choices = dialogueNode.Choices.ToList();
                
                nodeSO = dialogueNodeSO;
            }
            else if (node is DGActionNode actionNode)
            {
                var actionNodeSO = CreateAsset<DRActionNodeSO>(savePath, actionNode.Guid.ToString());
                actionNodeSO.TargetSO = actionNode.TargetSO;
                
                nodeSO = actionNodeSO;
            }
            else
                throw new ArgumentException($"Unexpected node type {node.GetType()} in the graph");

            nodeSO.Guid = node.Guid;
            nodeSO.IsStartingNode = node.IsStartingNode();
            nodeSO.NextGuids = node.NextGuids.ToList();

            rContainerSO.Nodes.Add(nodeSO);
            SaveAsset(nodeSO);

            return nodeSO;
        }

        private static void GetElementsFromGraphView(GraphView graphView, out List<DGNode> nodes,
            out List<DGGroup> groups)
        {
            nodes = new();
            groups = new();

            foreach (var graphElement in graphView.graphElements)
            {
                switch (graphElement)
                {
                    case DGNode node:
                        nodes.Add(node);
                        break;
                    case DGGroup group:
                        groups.Add(group);
                        break;
                }
            }
        }
        
        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            var foldersNames = path.Split('/');
            var parentFolderPath = string.Empty;
            foreach (var folderName in foldersNames)
            {
                if (!AssetDatabase.IsValidFolder(parentFolderPath + folderName))
                    AssetDatabase.CreateFolder(parentFolderPath[..^1], folderName);

                parentFolderPath += $"{folderName}/";
            }

            var fullPath = $"{path}/{assetName}.asset";
            var asset = LoadAsset<T>(path, assetName);

            if (asset != null) 
                return asset;
            
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject => 
            AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}