using System;
using ActionSystem;

namespace QuestSystem.Actions
{
    public class QuestAction : IAction
    {
        private readonly int _questId;
        private readonly string _title;
        private readonly string _task;
        
        private readonly QuestManager _questManager;

        public int Id { get; private set; }
        public bool Resolve { get; private set; } = true;
        
        public event Action<IAction> OnFinished;

        public QuestAction(int id, int questId, string title, string task, QuestManager questManager)
        {
            Id = id;
            _questId = questId;
            _title = title;
            _task = task;
            
            _questManager = questManager;
        }
        
        public void Perform()
        {
            _questManager.AddUpdate(_questId, _title, _task);
            Resolve = false;
            
            OnFinished?.Invoke(this);
        }
    }
}