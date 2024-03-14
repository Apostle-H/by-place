using UnityEngine.UIElements;

namespace Journal.Quest.View
{
    public class QuestFoldoutView
    {
        public VisualElement Root { get; private set; }

        private VisualTreeAsset _logLabelTree;

        public Foldout Foldout { get; private set; }

        public QuestFoldoutView(VisualElement root, string title)
        {
            Root = root;

            Foldout = Root.Q<Foldout>("QuestFoldout");
            Foldout.text = title;
            
            Foldout.AddToClassList("quest-foldout");
        }

        public void Log(string log)
        {
            var logLabel = new Label(log)
            {
                name = "LogLabel"
            };
            
            Foldout.Add(logLabel);
        }

        public void Close(string result)
        {
            Foldout.RemoveFromClassList("quest-foldout");
            Foldout.AddToClassList("completed-quest-foldout");
            
            Foldout.Clear();
            var resultLabel = new Label(result)
            {
                name = "Result Label"
            };

            Foldout.Add(resultLabel);
        }
    }
}