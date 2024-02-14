using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Elements;
using DialogueSystem.Utilities;
using DialogueSystem.Utils.Extensions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DSGraphView : GraphView
    {
        private DSEditorWindow _editorWindow;
        private DSSearchWindow _searchWindow;

        private MiniMap _miniMap;

        private Dictionary<int, DGGroup> _groups = new();

        public List<int> deletedNodesGuids = new();
        
        public Dictionary<int, string> renamedGroups = new();
        public List<string> deletedGroupsNames = new();

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            _editorWindow = dsEditorWindow;

            AddManipulators();
            AddGridBackground();
            AddSearchWindow();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) => 
            ports.Where(port => startPort.node != port.node && startPort.direction != port.direction).ToList();

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DNodeType.DIALOGUE));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Action)", DNodeType.ACTION));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (SetVariable)", DNodeType.SET_VARIABLE));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DNodeType nodeType)
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(menuEvent => menuEvent.menu
                .AppendAction(actionTitle, actionEvent =>
                {
                    switch (nodeType)
                    {
                        case DNodeType.DIALOGUE:
                            AddElement(CreateNode<DGDialogueNode>(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
                            break;
                        case DNodeType.ACTION:
                            AddElement(CreateNode<DGActionNode>(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
                            break;
                        case DNodeType.SET_VARIABLE:
                            AddElement(CreateNode<DGSetVariableNode>(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
                    }
                }
            ));

            return contextualMenuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(menuEvent => menuEvent.menu
                .AppendAction("Add Group", actionEvent => 
                    CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        public DGGroup CreateGroup(string title, Vector2 position, int guid = -1)
        {
            var group = new DGGroup(title, position);
            if (guid != -1)
                group.Guid = guid;
            AddGroup(group);
            AddElement(group);

            foreach (var selectedElement in selection)
            {
                if (!(selectedElement is DGNode node))
                    continue;

                group.AddElement(node);
            }

            return group;
        }

        public T CreateNode<T>(Vector2 position, int guid = -1, bool draw = true) where T : DGNode
        {
            var node = Activator.CreateInstance<T>();
            node.Initialize(this, position);
            if (guid != -1)
                node.Guid = guid;
            if (draw)
                node.Draw();

            return node;
        }

        private void OnElementsDeleted() => deleteSelection = (operationName, askUser) =>
            {
                var groupsToDelete = new List<DGGroup>();
                var nodesToDelete = new List<DGNode>();
                var edgesToDelete = new List<Edge>();

                foreach (var selectedElement in selection)
                {
                    switch (selectedElement)
                    {
                        case DGNode node:
                            nodesToDelete.Add(node);
                            continue;
                        case Edge edge:
                            edgesToDelete.Add(edge);
                            continue;
                        case DGGroup group:
                            groupsToDelete.Add(group);
                            continue;
                    }
                }
                
                DeleteElements(edgesToDelete);
                
                foreach (var nodeToDelete in nodesToDelete)
                {
                    if (nodeToDelete.GroupGuid != -1)
                    {
                        _groups[nodeToDelete.GroupGuid].RemoveElement(nodeToDelete);
                    }

                    nodeToDelete.DisconnectAllPorts();
                    deletedNodesGuids.Add(nodeToDelete.Guid);
                }
                DeleteElements(nodesToDelete);

                foreach (var groupToDelete in groupsToDelete)
                {
                    RemoveGroup(groupToDelete);
                    RemoveElement(groupToDelete);
                }
            };

        private void OnGroupElementsAdded() => elementsAddedToGroup = (group, nodes) =>
            {
                foreach (var node in nodes)
                {
                    var dNode = (DGNode)node;
                    deletedNodesGuids.Add(dNode.Guid);
                    ((DGGroup)group).AddNode((DGNode)dNode);
                }
            };

        private void OnGroupElementsRemoved() => elementsRemovedFromGroup = (group, nodes) =>
            {
                foreach (var node in nodes)
                    ((DGGroup)group).RemoveNode((DGNode)node);
            };

        private void OnGroupRenamed() => groupTitleChanged = (group, newTitle) =>
        {
            var dsGroup = (DGGroup)group;
            if (!renamedGroups.ContainsKey(dsGroup.Guid))
                renamedGroups.Add(dsGroup.Guid, dsGroup.PreviousName);
            else
                renamedGroups[dsGroup.Guid] = dsGroup.PreviousName;
            
            dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
            if (deletedGroupsNames.Contains(dsGroup.title))
                deletedGroupsNames.Remove(dsGroup.title);
        };

        private void OnGraphViewChanged() => graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (var edge in changes.edgesToCreate)
                    {
                        var outputData = (DOutputData)edge.output.userData;
                        outputData.NextGuid = ((DGNode)edge.input.node).Guid;
                    }
                }

                if (changes.elementsToRemove == null) 
                    return changes;
                
                foreach (var element in changes.elementsToRemove)
                {
                    if (element is not Edge edge)
                        continue;

                    var choiceData = (DOutputData)edge.output.userData;
                    choiceData.NextGuid = -1;
                }
                
                return changes;
            };
        
        public void AddGroup(DGGroup group)
        {
            _groups.Add(group.Guid, group);
            if (deletedGroupsNames.Contains(group.title))
                deletedGroupsNames.Remove(group.title);
        }

        public void RemoveGroup(DGGroup group)
        {
            _groups.Remove(group.Guid);
            if (renamedGroups.ContainsKey(group.Guid))
                renamedGroups.Remove(group.Guid);
            deletedGroupsNames.Add(group.title);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddSearchWindow()
        {
            if (_searchWindow == null)
                _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();

            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/Styles/DSGraphViewStyles.uss",
                "DialogueSystem/Styles/DSNodeStyles.uss"
            );
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
                worldMousePosition = _editorWindow.rootVisualElement
                    .ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, mousePosition - _editorWindow.position.position);

            return contentViewContainer.WorldToLocal(worldMousePosition);
        }

        public void ClearGraph()
        {
            graphElements.ForEach(RemoveElement);
            _groups.Clear();
        }
    }
}