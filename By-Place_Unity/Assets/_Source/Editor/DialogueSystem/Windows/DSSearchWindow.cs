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
                    userData = new DGDialogueNode(),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Action", _indentationIcon))
                {
                    userData = new DGActionNode(),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Set Variable", _indentationIcon))
                {
                    userData = new DGSetVariableNode(),
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Group", _indentationIcon))
                {
                    userData = new Group(),
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
                    var dialogueNode = _graphView.CreateNode<DGDialogueNode>(localMousePosition);
                    _graphView.AddElement(dialogueNode);
                    return true;
                case DGActionNode _:
                    var actionNode = _graphView.CreateNode<DGActionNode>(localMousePosition);
                    _graphView.AddElement(actionNode);
                    return true;
                case DGSetVariableNode _:
                    var setVariableNode = _graphView.CreateNode<DGSetVariableNode>(localMousePosition);
                    _graphView.AddElement(setVariableNode);
                    return true;
                case Group _:
                    _graphView.CreateGroup("DialogueGroup", localMousePosition);
                    return true;
                default:
                    return false;
            }
        }
    }
}