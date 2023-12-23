using DialogueSystem.Utils;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DSGroup : Group
    {
        public int Guid { get; set; } = -1;

        public DSGroup(string groupTitle, Vector2 position)
        {
            Guid = IDGenerator.NewId();

            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }
    }
}