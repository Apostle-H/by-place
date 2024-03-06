using QuestSystem.Data;
using UnityEngine.UIElements;

namespace QuestSystem.View
{
    public class QuestView
    {
        private Label _title;
        private Label _task;
        
        public VisualElement Root { get; private set; }

        public QuestView(VisualElement root)
        {
            Root = root;

            _title = Root.Q<Label>("TitleLabel");
            _task = Root.Q<Label>("TaskLabel");
        }
        
        public void Show() => Root.style.display = DisplayStyle.Flex;

        public void Hide() => Root.style.display = DisplayStyle.None;

        public void Update(string title, string task)
        {
            _title.text = title;
            _task.text = task;
        }
    }
}