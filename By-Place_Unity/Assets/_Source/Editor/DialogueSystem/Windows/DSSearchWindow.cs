using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Windows
{
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DSGraphView _graphView;
        private Texture2D _indentationIcon;

        public void Initialize(DSGraphView dsGraphView)
        {
            _graphView = dsGraphView;

            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, Color.clear);
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create")),
                new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
                {
                    userData = new DGDialogueNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Action", _indentationIcon))
                {
                    userData = new DGActionNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Set Variable", _indentationIcon))
                {
                    userData = new DGSetVariableNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Branch", _indentationIcon))
                {
                    userData = new DGBranchNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Group", _indentationIcon))
                {
                    userData = new DGGroup(default, default),
                    level = 1
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (searchTreeEntry.userData)
            {
                case DGDialogueNode _:
                    var dialogueNode = new DGDialogueNode(localMousePosition);
                    dialogueNode.GraphView = _graphView;
                    _graphView.AddNode(dialogueNode);
                    return true;
                case DGActionNode _:
                    var actionNode = new DGActionNode(localMousePosition);
                    _graphView.AddNode(actionNode);
                    return true;
                case DGSetVariableNode _:
                    var setVariableNode = new DGSetVariableNode(localMousePosition);
                    _graphView.AddNode(setVariableNode);
                    return true;
                case DGBranchNode _:
                    var branchNode = new DGBranchNode(localMousePosition);
                    _graphView.AddNode(branchNode);
                    return true;
                case DGGroup _:
                    var group = new DGGroup("Dialogue Group", localMousePosition);
                    _graphView.AddGroup(group);
                    return true;
                default:
                    return false;
            }
        }
    }
}