using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DSGroup : Group
    {
        public GUID Guid { get; set; } = new();

        public DSGroup(string groupTitle, Vector2 position)
        {
            Guid = GUID.Generate();

            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }
    }
}