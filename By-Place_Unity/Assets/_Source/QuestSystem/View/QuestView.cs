using UnityEngine;
using UnityEngine.UIElements;

namespace QuestSystem.View
{
    public class QuestView
    {
        private Label _title;
        private Label _task;
        
        public Quest TargetQuest { get; private set; }
        public VisualElement Root { get; private set; }
        public int IndexInManager { get; private set; }

        public QuestView(VisualTreeAsset questTree, int indexInManager)
        {
            Root = questTree.CloneTree().Q<VisualElement>("Root");
            IndexInManager = indexInManager;

            _title = Root.Q<Label>("TitleLabel");
            _task = Root.Q<Label>("TaskLabel");
        }
        
        public void Show() => Root.style.display = DisplayStyle.Flex;

        public void Hide() => Root.style.display = DisplayStyle.None;

        public void SetTargetQuest(Quest quest)
        {
            if (TargetQuest != default)
                TargetQuest.OnUpdate -= Update;
            TargetQuest = quest;
            if (TargetQuest != default)
                TargetQuest.OnUpdate += Update;
        }

        private void Update()
        {
            _title.text = TargetQuest.Title;
            _task.text = TargetQuest.Task;
        }
    }
}