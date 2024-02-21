using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data.NodeParams;
using DialogueSystem.Elements;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Data
{
    public class DEContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DEGroup> Groups { get; private set; } = new();
        [field: SerializeField] public List<DENode> Nodes { get; private set; } = new();

        [OnOpenAsset]
        public static bool Open(int instanceId, int line)
        {
            var graph = EditorUtility.InstanceIDToObject(instanceId) as DEContainerSO;
            if (graph != null)
            {
                
                return true;
            }

            return false;
        }
        
        public void Save(List<DGGroup> groups, List<DGNode> nodes)
        {
            SaveGroups(groups);
            SaveNodes(nodes);
        }
        
        private void SaveGroups(List<DGGroup> groups)
        {
            foreach (var group in groups)
            {
                var groupSave = new DEGroup();
                groupSave.Save(group);
                
                Groups.Add(groupSave);
            }
        }
        
        private void SaveNodes(List<DGNode> nodes)
        {
            foreach (var node in nodes)
            {
                var nodeSave = new DENode();
                nodeSave.Save(node);
                
                Nodes.Add(nodeSave);
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
                var group = groupSave.Load();
                graphView.AddGroup(group);
                
                loadedGroups.Add(group.Guid, group);
            }
        }
        
        private void LoadNodes(DSGraphView graphView, Dictionary<int, DGGroup> loadedGroups, 
            ref Dictionary<int, DGNode> loadedNodes)
        {
            foreach (var nodeSave in Nodes)
            {
                var node = nodeSave.Load(graphView);
                
                graphView.AddNode(node);
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