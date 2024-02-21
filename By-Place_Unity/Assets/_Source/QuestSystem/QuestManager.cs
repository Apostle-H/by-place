using System;
using System.Collections.Generic;
using QuestSystem.Data;

namespace QuestSystem
{
    public class QuestManager
    {
        private Dictionary<int, Quest> _quests = new();

        public event Action<Quest> OnAdd;
        public event Action<Quest> OnRemove;
        
        public void AddUpdate(int id, string title, string task)
        {
            if (!_quests.ContainsKey(id))
                Add(id, title, task);
            
            _quests[id].Update(title, task);
        }
        
        private void Add(int id, string title, string task)
        {
            _quests.Add(id, new Quest(id, title, task));
            
            OnAdd?.Invoke(_quests[id]);
        }

        public void Remove(int id)
        {
            if (!_quests.ContainsKey(id))
                return;

            var quest = _quests[id];
            _quests.Remove(id);

            OnRemove?.Invoke(quest);
        }
    }
}