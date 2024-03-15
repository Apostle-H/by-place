using System;
using ActionSystem;
using VContainer;

namespace QuestSystem.Actions
{
    public class UpdateQuestAction : IAction
    {
        private readonly int _questId;
        private readonly string _task;
        private readonly string _conclusion;
        
        private QuestManager _questManager;

        public int Id { get; private set; }
        public bool Resolvable { get; set; } = true;
        
        public event Action<IAction> OnFinished;

        public UpdateQuestAction(int id, int questId, string task, string conclusion)
        {
            Id = id;
            _questId = questId;
            _task = task;
            _conclusion = conclusion;
        }

        [Inject]
        private void Inject(QuestManager questManager) => _questManager = questManager;
        
        public void Resolve()
        {
            _questManager.Update(_questId, _task, _conclusion);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }

        public void Skip() { }
    }
}