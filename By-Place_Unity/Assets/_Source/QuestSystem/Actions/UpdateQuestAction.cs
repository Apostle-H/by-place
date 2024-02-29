using System;
using ActionSystem;
using VContainer;

namespace QuestSystem.Actions
{
    public class UpdateQuestAction : IAction
    {
        private readonly int _questId;
        private readonly string _title;
        private readonly string _task;
        private readonly string _conclusion;
        
        private QuestManager _questManager;

        public int Id { get; private set; }
        public bool Resolve { get; private set; } = true;
        
        public event Action<IAction> OnFinished;

        public UpdateQuestAction(int id, int questId, string title, string task, string conclusion)
        {
            Id = id;
            _questId = questId;
            _title = title;
            _task = task;
            _conclusion = conclusion;
        }

        [Inject]
        private void Inject(QuestManager questManager) => _questManager = questManager;
        
        public void Perform()
        {
            _questManager.Update(_questId, _title, _task, _conclusion);
            Resolve = false;
            
            OnFinished?.Invoke(this);
        }
    }
}