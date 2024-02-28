using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Elements;
using DialogueSystem.Elements.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Windows
{
    public class DSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DGraphView _graphView;
        private Texture2D _indentationIcon;

        public void Initialize(DGraphView graphView)
        {
            _graphView = graphView;

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
                    userData = new DDialogueNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Action", _indentationIcon))
                {
                    userData = new DActionNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Set Variable", _indentationIcon))
                {
                    userData = new DSetVariableNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Branch", _indentationIcon))
                {
                    userData = new DBranchNode(default, default),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Group", _indentationIcon))
                {
                    userData = new DGroup(default, default),
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
                case DDialogueNode _:
                    var dialogueNode = new DDialogueNode(localMousePosition);
                    dialogueNode.GraphView = _graphView;
                    _graphView.AddNode(dialogueNode);
                    return true;
                case DActionNode _:
                    var actionNode = new DActionNode(localMousePosition);
                    _graphView.AddNode(actionNode);
                    return true;
                case DSetVariableNode _:
                    var setVariableNode = new DSetVariableNode(localMousePosition);
                    _graphView.AddNode(setVariableNode);
                    return true;
                case DBranchNode _:
                    var branchNode = new DBranchNode(localMousePosition);
                    _graphView.AddNode(branchNode);
                    return true;
                case DGroup _:
                    var group = new DGroup("Dialogue Group", localMousePosition);
                    _graphView.AddGroup(group);
                    return true;
                default:
                    return false;
            }
        }
    }
}