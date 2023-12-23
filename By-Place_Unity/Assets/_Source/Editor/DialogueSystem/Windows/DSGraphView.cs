using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Elements;
using DialogueSystem.Utilities;
using DialogueSystem.Utils.Extensions;
using UnityEditor;
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

        private Dictionary<int, DSNode> _ungroupedNodes = new();
        private Dictionary<int, DSGroup> _groups = new();
        private Dictionary<int, List<int>> _groupedNodes = new();

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

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", typeof(DSSingleChoiceNode)));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", typeof(DSMultipleChoiceNode)));
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, Type nodeType)
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(menuEvent => menuEvent.menu
                .AppendAction(actionTitle, actionEvent => 
                    AddElement(CreateNode(nodeType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

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

        public DSGroup CreateGroup(string title, Vector2 position, int guid = -1)
        {
            var group = new DSGroup(title, position);
            if (guid != -1)
                group.Guid = guid;
            AddGroup(group);
            AddElement(group);

            foreach (var selectedElement in selection)
            {
                if (!(selectedElement is DSNode node))
                    continue;

                group.AddElement(node);
            }

            return group;
        }

        public DSNode CreateNode(Type nodeType, Vector2 position, int guid = -1, bool draw = true)
        {
            var node = (DSNode) Activator.CreateInstance(nodeType);
            node.Initialize(this, position);
            if (guid != -1)
                node.Guid = guid;
            if (draw)
                node.Draw();

            AddUngroupedNode(node);
            return node;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                var groupsToDelete = new List<DSGroup>();
                var nodesToDelete = new List<DSNode>();
                var edgesToDelete = new List<Edge>();

                foreach (var selectedElement in selection)
                {
                    switch (selectedElement)
                    {
                        case DSNode node:
                            nodesToDelete.Add(node);
                            continue;
                        case Edge edge:
                            edgesToDelete.Add(edge);
                            continue;
                        case DSGroup group:
                            groupsToDelete.Add(group);
                            continue;
                    }
                }
                
                DeleteElements(edgesToDelete);
                
                foreach (var nodeToDelete in nodesToDelete)
                {
                    if (nodeToDelete.Group != null)
                    {
                        _groupedNodes.Remove(nodeToDelete.Guid);
                        nodeToDelete.Group.RemoveElement(nodeToDelete);
                    }

                    _ungroupedNodes.Remove(nodeToDelete.Guid);
                    nodeToDelete.DisconnectAllPorts();
                }
                DeleteElements(nodesToDelete);

                foreach (var groupToDelete in groupsToDelete)
                {
                    RemoveGroup(groupToDelete);
                    RemoveElement(groupToDelete);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is not DSNode node)
                        continue;
                    
                    AddGroupedNode((DSGroup)group, node);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is not DSNode node)
                        continue;
                    
                    AddUngroupedNode(node);
                }
            };
        }

        private void OnGroupRenamed() => groupTitleChanged = (group, newTitle) => 
            ((DSGroup)group).title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (var edge in changes.edgesToCreate)
                    {
                        var nextNode = (DSNode)edge.input.node;
                        var choiceData = (DSNodeChoiceSave)edge.output.userData;
                        choiceData.NextNodeGuid = nextNode.Guid;
                    }
                }

                if (changes.elementsToRemove == null) 
                    return changes;
                
                foreach (var element in changes.elementsToRemove)
                {
                    if (element is not Edge edge)
                        continue;

                    var choiceData = (DSNodeChoiceSave)edge.output.userData;
                    choiceData.NextNodeGuid = -1;
                }
                
                return changes;
            };
        }

        public void AddUngroupedNode(DSNode node)
        {
            _groupedNodes.Remove(node.Guid);
            node.Group = default;
            _ungroupedNodes.Add(node.Guid, node);
        }

        public void AddGroup(DSGroup group)
        {
            _groups.Add(group.Guid, group);
            _groupedNodes.Add(group.Guid, new());
        }

        public void RemoveGroup(DSGroup group)
        {
            _groups.Remove(group.Guid);
            _groupedNodes.Remove(group.Guid);
        }

        public void AddGroupedNode(DSGroup group, DSNode node)
        {
            _ungroupedNodes.Remove(node.Guid);
            node.Group = group;
            _groupedNodes[group.Guid].Add(node.Guid);
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
            _groupedNodes.Clear();
            _ungroupedNodes.Clear();
        }
    }
}