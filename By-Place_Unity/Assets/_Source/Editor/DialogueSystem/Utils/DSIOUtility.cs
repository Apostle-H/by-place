using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Elements;
using DialogueSystem.Scripts.Data;
using DialogueSystem.Scripts.ScriptableObjects;
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
            
            AssetDatabase.DeleteAsset(containerPath);
            var graphSaveSO = CreateAsset<DSGraphSaveSO>(DS_GRAPH_SAVE_DATA_SO_PATH, $"{graphName}Graph");
            var dialogueContainerSO = CreateAsset<DialogueContainerSO>(containerPath, graphName);
            
            SaveNodes(graphSaveSO, dialogueContainerSO, containerPath, nodes, out var groupedNodes);
            SaveGroups(graphSaveSO, dialogueContainerSO, containerPath, groups, groupedNodes);
            
            graphSaveSO.FileName = graphName;
            SaveAsset(graphSaveSO);
            dialogueContainerSO.FileName = graphName;
            SaveAsset(dialogueContainerSO);
        }
        
        public static void Load(DSGraphView graphView, string graphName)
        {
            var graphData = LoadAsset<DSGraphSaveSO>(DS_GRAPH_SAVE_DATA_SO_PATH, graphName);

            if (graphData == null)
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

            DSEditorWindow.UpdateFileName(graphData.FileName);

            var loadedGroups = new Dictionary<GUID, DSGroup>();
            LoadGroups(graphData.Groups, graphView, ref loadedGroups);
            var loadedNodes = new Dictionary<GUID, DSNode>();
            LoadNodes(graphData.Nodes, graphView, loadedGroups, ref loadedNodes);
            LoadNodesConnections(graphView, loadedNodes);
        }
        
        private static void SaveNodes(DSGraphSaveSO graphSaveSO, DialogueContainerSO dialogueContainerSO, 
            string containerPath, List<DSNode> nodes, out Dictionary<GUID, List<DialogueNodeSO>> groupedNodesSOs)
        {
            groupedNodesSOs = new Dictionary<GUID, List<DialogueNodeSO>>();
            
            var nodesSOs = new Dictionary<GUID, DialogueNodeSO>();
            foreach (var node in nodes)
            {
                SaveNodeGraph(graphSaveSO, node);
                SaveNodeContainer(dialogueContainerSO, containerPath, node, ref nodesSOs, groupedNodesSOs);
            }

            FilterDialogueChoices(nodes, nodesSOs);
        }

        private static void SaveNodeGraph(DSGraphSaveSO graphSaveSO, DSNode node) =>
            graphSaveSO.Nodes.Add(new DSNodeSave()
            {
                Guid = node.Guid,
                Choices = CloneNodeChoices(node.Choices),
                Text = node.Text,
                GroupGuid = node.Group?.Guid ?? new GUID(),
                DialogueType = node.DialogueType,
                Position = node.GetPosition().position,
                SpeakerSO = node.SpeakerSO
            });

        private static void SaveNodeContainer(DialogueContainerSO dialogueContainerSO, string containerPath, DSNode node,
            ref Dictionary<GUID, DialogueNodeSO> nodesSOs, Dictionary<GUID, List<DialogueNodeSO>> groupedNodesSOs)
        {
            var hasGroup = node.Group != default;
            var savePath = hasGroup
                ? $"{containerPath}/{node.Group.title}/Nodes"
                : $"{containerPath}/Ungrouped/";
            
            var nodeSO = CreateAsset<DialogueNodeSO>(savePath, node.Guid.ToString());
            nodeSO.Guid = node.Guid;
            nodeSO.SpeakerSO = node.SpeakerSO;
            nodeSO.Text = node.Text;
            nodeSO.Choices = ConvertNodeChoicesToDialogueChoices(node.Choices);
            nodeSO.DialogueType = node.DialogueType;
            nodeSO.IsStartingDialogue = node.IsStartingNode();
            
            nodesSOs.Add(node.Guid, nodeSO);
            
            if (hasGroup)
            {
                groupedNodesSOs.TryAdd(node.Group.Guid, new List<DialogueNodeSO>());
                groupedNodesSOs[node.Group.Guid].Add(nodeSO);
            }
            else
                dialogueContainerSO.UngroupedDialogues.Add(nodeSO);
            
            SaveAsset(nodeSO);
        }

        private static void FilterDialogueChoices(List<DSNode> nodes, Dictionary<GUID, DialogueNodeSO> nodesSOs)
        {
            foreach (var node in nodes)
            {
                var dialogue = nodesSOs[node.Guid];
                for (var choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    var nodeChoice = node.Choices[choiceIndex];
                    if (nodeChoice.NextNodeGuid.Empty())
                        continue;

                    dialogue.Choices[choiceIndex].NextNode = nodesSOs[nodeChoice.NextNodeGuid];
                    SaveAsset(dialogue);
                }
            }
        }
        
        private static List<DialogueNodeChoice> ConvertNodeChoicesToDialogueChoices(List<DSNodeChoiceSave> nodeChoicesSaves)
        {
            var dialogueChoices = new List<DialogueNodeChoice>();
            foreach (var nodeChoiceSave in nodeChoicesSaves)
            {
                var choiceData = new DialogueNodeChoice() { Text = nodeChoiceSave.Text };
                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }
        
        private static void SaveGroups(DSGraphSaveSO graphSaveSO, DialogueContainerSO dialogueContainerSO, 
            string containerPath, List<DSGroup> groups, Dictionary<GUID, List<DialogueNodeSO>> groupedNodesSOs)
        {
            foreach (var group in groups)
            {
                SaveGroupGraph(graphSaveSO, group);
                SaveGroupContainer(dialogueContainerSO, containerPath, group, groupedNodesSOs);
            }
        }

        private static void SaveGroupGraph(DSGraphSaveSO graphSaveSO, DSGroup group) =>
            graphSaveSO.Groups.Add(new DSGroupSave()
            {
                Guid = group.Guid,
                Name = group.title,
                Position = group.GetPosition().position
            });

        private static void SaveGroupContainer(DialogueContainerSO dialogueContainerSO, string containerPath,
            DSGroup group, Dictionary<GUID, List<DialogueNodeSO>> groupedNodesSOs)
        {
            var groupSO = CreateAsset<DialogueGroupSO>($"{containerPath}/{group.title}", group.title);
            groupSO.Guid = group.Guid;
            groupSO.Name = group.title;
            if (groupedNodesSOs.TryGetValue(group.Guid, out var groupNodesSOs))
            {
                foreach (var nodeSO in groupNodesSOs)
                {
                    if (nodeSO.IsStartingDialogue)
                        groupSO.StartingNode = nodeSO;
                    
                    groupSO.NodesSOs.Add(nodeSO.Guid, nodeSO);
                }
            }
            
            dialogueContainerSO.GroupsSOs.Add(groupSO);

            SaveAsset(groupSO);
        }

        private static void LoadGroups(List<DSGroupSave> groupsSaves, DSGraphView graphView, 
            ref Dictionary<GUID, DSGroup> loadedGroups)
        {
            foreach (var groupData in groupsSaves)
            {
                var group = graphView.CreateGroup(groupData.Name, groupData.Position, groupData.Guid);
                group.title = groupData.Name;
                
                loadedGroups.Add(group.Guid, group);
            }
        }

        private static void LoadNodes(List<DSNodeSave> nodesSaves, DSGraphView graphView, 
            Dictionary<GUID, DSGroup> loadedGroups, ref Dictionary<GUID, DSNode> loadedNodes)
        {
            foreach (var nodeSave in nodesSaves)
            {
                Type nodeType = default;
                switch (nodeSave.DialogueType)
                {
                    case DialogueType.SINGLE_CHOICE:
                        nodeType = typeof(DSSingleChoiceNode);
                        break;
                    case DialogueType.MULTIPLE_CHOICE:
                        nodeType = typeof(DSMultipleChoiceNode);
                        break;
                }
                var node = graphView.CreateNode(nodeType, nodeSave.Position, nodeSave.Guid, false);
                node.SpeakerSO = nodeSave.SpeakerSO;
                node.Choices = CloneNodeChoices(nodeSave.Choices);
                node.Text = nodeSave.Text;
                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.Guid, node);

                if (nodeSave.GroupGuid.Empty())
                    continue;

                var group = loadedGroups[nodeSave.GroupGuid];
                node.Group = group;
                
                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections(DSGraphView graphView, Dictionary<GUID, DSNode> loadedNodes)
        {
            foreach (KeyValuePair<GUID, DSNode> loadedNode in loadedNodes)
            {
                foreach (var visualElement in loadedNode.Value.outputContainer.Children())
                {
                    var choicePort = (Port)visualElement;
                    var choiceData = (DSNodeChoiceSave)choicePort.userData;

                    if (choiceData.NextNodeGuid.Empty())
                        continue;

                    var nextNode = loadedNodes[choiceData.NextNodeGuid];
                    var nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                    var edge = choicePort.ConnectTo(nextNodeInputPort);
                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        private static void GetElementsFromGraphView(GraphView graphView, out List<DSNode> nodes,
            out List<DSGroup> groups)
        {
            nodes = new();
            groups = new();

            foreach (var graphElement in graphView.graphElements)
            {
                switch (graphElement)
                {
                    case DSNode node:
                        nodes.Add(node);
                        break;
                    case DSGroup group:
                        groups.Add(group);
                        break;
                }
            }
        }
        
        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            var foldersNames = path.Split('/');
            var parentFolderPath = String.Empty;
            foreach (var folderName in foldersNames)
            {
                if (!AssetDatabase.IsValidFolder(parentFolderPath + folderName))
                    AssetDatabase.CreateFolder(parentFolderPath[..^1], folderName);

                parentFolderPath += $"{folderName}/";
            }

            var fullPath = $"{path}/{assetName}.asset";
            var asset = LoadAsset<T>(path, assetName);

            if (asset != null)
                AssetDatabase.DeleteAsset(fullPath);
                
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

        private static List<DSNodeChoiceSave> CloneNodeChoices(List<DSNodeChoiceSave> nodeChoices)
        {
            var choices = new List<DSNodeChoiceSave>();

            foreach (var choice in nodeChoices)
            {
                var choiceData = new DSNodeChoiceSave()
                {
                    Text = choice.Text,
                    NextNodeGuid = choice.NextNodeGuid
                };

                choices.Add(choiceData);
            }

            return choices;
        }
    }
}