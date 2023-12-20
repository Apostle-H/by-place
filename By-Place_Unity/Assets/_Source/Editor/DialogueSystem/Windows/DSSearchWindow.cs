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
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create")),
                new SearchTreeEntry(new GUIContent("Single Choice", _indentationIcon))
                {
                    userData = DialogueType.SINGLE_CHOICE,
                    level = 1
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
                {
                    userData = DialogueType.MULTIPLE_CHOICE,
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
            Vector2 localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (searchTreeEntry.userData)
            {
                case DialogueType.SINGLE_CHOICE:
                    DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode) _graphView
                        .CreateNode(typeof(DSSingleChoiceNode), localMousePosition);

                    _graphView.AddElement(singleChoiceNode);
                    return true;
                case DialogueType.MULTIPLE_CHOICE:
                    DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode) _graphView
                        .CreateNode(typeof(DSMultipleChoiceNode), localMousePosition);

                    _graphView.AddElement(multipleChoiceNode);
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