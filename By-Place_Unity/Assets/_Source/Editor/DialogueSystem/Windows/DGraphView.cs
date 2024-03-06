using System.Collections.Generic;
using System.Linq;
using Dialogue.Data.NodeParams;
using DialogueSystem.Elements;
using DialogueSystem.Elements.Nodes;
using DialogueSystem.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Extensions;

namespace DialogueSystem.Windows
{
    public class DGraphView : GraphView
    {
        private DGEditorWindow _editorWindow;
        private DSearchWindow _searchWindow;

        private MiniMap _miniMap;

        private Dictionary<int, DGroup> _groups = new();

        public HashSet<int> movedNodesGuids = new();
        public HashSet<int> deletedNodesRuntimeIds = new();
        
        public HashSet<int> renamedGroupsGuids = new();
        public HashSet<int> deletedGroupGuids = new();

        public DGraphView(DGEditorWindow dgEditorWindow)
        {
            _editorWindow = dgEditorWindow;

            AddManipulators();
            AddGridBackground();
            AddSearchWindow();

            deleteSelection = ElementsDeleted;
            elementsAddedToGroup = GroupElementsAdded;
            elementsRemovedFromGroup = GroupElementsRemoved;
            groupTitleChanged = GroupRenamed;
            graphViewChanged =  OnGraphViewChanged;

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
        }

        public void AddGroup(DGroup group)
        {
            AddElement(group);
            _groups.Add(group.Guid, group);
            
            foreach (var selectedElement in selection)
            {
                if (selectedElement is not DNode node)
                    continue;

                group.AddElement(node);
            }
            
            deletedGroupGuids.Remove(group.RuntimeAssetId);
        }
        
        public void RemoveGroup(DGroup group)
        {
            group.RemoveElements(group.Nodes.Values.ToList());
            _groups.Remove(group.Guid);
            
            renamedGroupsGuids.Remove(group.Guid);
            if (group.RuntimeAssetId != int.MaxValue)
                deletedGroupGuids.Add(group.RuntimeAssetId);
        }

        public void AddNode(DNode node)
        {
            AddElement(node);
            node.Draw();
            
            deletedNodesRuntimeIds.Remove(node.RuntimeAssetId);
        }

        public void RemoveNode(DNode node)
        {
            node.DisconnectAllPorts(this);
            
            movedNodesGuids.Remove(node.Guid);
            if (node.RuntimeAssetId != int.MaxValue)
                deletedNodesRuntimeIds.Add(node.RuntimeAssetId);
        }

        private void ElementsDeleted(string operationName, AskUser askUser)
        {
            var groupsToDelete = selection.Where(element => element is DGroup).Cast<DGroup>();
            var nodesToDelete = selection.Where(element => element is DNode).Cast<DNode>();
            var edgesToDelete = selection.Where(element => element is Edge).Cast<Edge>();

            DeleteElements(edgesToDelete);
            foreach (var nodeToDelete in nodesToDelete)
                RemoveNode(nodeToDelete);
            DeleteElements(nodesToDelete);
            foreach (var groupToDelete in groupsToDelete) 
                RemoveGroup(groupToDelete);
            DeleteElements(groupsToDelete);
        }

        private void GroupElementsAdded(Group group, IEnumerable<GraphElement> groupNodes)
        {
            foreach (var node in groupNodes)
            {
                var gNode = (DNode)node;
                ((DGroup)group).AddNode(gNode);

                if (gNode.RuntimeAssetId != int.MaxValue)
                    movedNodesGuids.Add(gNode.Guid);
            }
        }

        private void GroupElementsRemoved(Group group, IEnumerable<GraphElement> groupNodes)
        {
            foreach (var node in groupNodes)
            {
                var gNode = (DNode)node;
                ((DGroup)group).RemoveNode(gNode);
                
                if (!deletedNodesRuntimeIds.Contains(gNode.RuntimeAssetId) && gNode.RuntimeAssetId != int.MaxValue)
                    movedNodesGuids.Add(gNode.Guid);
            }
        }

        private void GroupRenamed(Group group, string newTitle)
        {
            var gGroup = (DGroup)group;
            gGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
            
            if (gGroup.RuntimeAssetId == int.MaxValue)
                return;

            renamedGroupsGuids.Add(gGroup.Guid);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    var outputData = (DOutputData)edge.output.userData;
                    outputData.NextGuid = ((DNode)edge.input.node).Guid;
                }
            }

            if (change.elementsToRemove == null) 
                return change;
            
            foreach (var element in change.elementsToRemove)
            {
                if (element is not Edge edge)
                    continue;

                var choiceData = (DOutputData)edge.output.userData;
                choiceData.NextGuid = -1;
            }
            
            return change;
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
                _searchWindow = ScriptableObject.CreateInstance<DSearchWindow>();

            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddStyles() => 
            this.AddStyleSheets("DialogueSystem/Styles/DSGraphViewStyles.uss", "DialogueSystem/Styles/DSNodeStyles.uss");

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

        public void GetElements(out List<DNode> nodes, out List<DGroup> groups)
        {
            nodes = graphElements.Where(element => element is DNode).Cast<DNode>().ToList();
            groups = graphElements.Where(element => element is DGroup).Cast<DGroup>().ToList();
        }
    }
}