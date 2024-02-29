using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Journal.Quest.View
{
    public class QuestFoldoutView
    {
        public VisualElement Root { get; private set; }

        private VisualTreeAsset _logLabelTree;

        private Foldout _foldout;

        public QuestFoldoutView(VisualElement root, string title)
        {
            Root = root;

            _foldout = Root.Q<Foldout>("QuestFoldout");
            _foldout.text = title;
            
            _foldout.AddToClassList("quest-foldout");
        }

        public void Log(string log)
        {
            var logLabel = new Label(log)
            {
                name = "LogLabel"
            };

            _foldout.Add(logLabel);
        }

        public void Close(string result)
        {
            _foldout.RemoveFromClassList("quest-foldout");
            _foldout.AddToClassList("completed-quest-foldout");
            
            _foldout.Clear();
            var resultLabel = new Label(result)
            {
                name = "Result Label"
            };

            _foldout.Add(resultLabel);
        }
    }
}