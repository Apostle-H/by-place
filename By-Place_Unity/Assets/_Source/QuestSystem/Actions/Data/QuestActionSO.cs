using ActionSystem;
using ActionSystem.Data;
using QuestSystem.Data;
using UnityEngine;

namespace QuestSystem.Actions.Data
{
    public abstract class QuestActionSO : ActionSO
    {
        [SerializeField] private QuestSO targetQuestSO;
        
        public int QuestId => targetQuestSO.Id;

        public abstract IAction Build();
    }
}