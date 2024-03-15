using System;
using ActionSystem;
using VContainer;

namespace QuestSystem.Actions
{
    public class FinishQuestAction : IAction
    {
        private readonly int _questId;
        private readonly string _result;
        
        private QuestManager _questManager;

        public int Id { get; private set; }
        public bool Resolvable { get; set; } = true;
        
        public event Action<IAction> OnFinished;

        public FinishQuestAction(int id, int questId, string result)
        {
            Id = id;
            _questId = questId;
            _result = result;
        }

        [Inject]
        private void Inject(QuestManager questManager) => _questManager = questManager;
        
        public void Resolve()
        {
            _questManager.Close(_questId, _result);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }
        
        public void Skip() { }
    }
}