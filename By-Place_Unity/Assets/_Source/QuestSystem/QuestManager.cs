using System;
using System.Collections.Generic;
using Journal.Quest;
using QuestSystem.Data;
using SaveLoad;
using VContainer;

namespace QuestSystem
{
    public class QuestManager
    {
        private JournalQuests _journalQuests;
        
        private Dictionary<int, Quest> _quests = new();

        public string Path => "Quests";
        
        public event Action<int> OnOpen;
        /// <summary>
        /// -questId, title, task-
        /// </summary>
        public event Action<int, string, string> OnUpdate;
        public event Action<int> OnClose;

        [Inject]
        public QuestManager(JournalQuests journalQuests) => _journalQuests = journalQuests;

        public void Update(int questId, string title, string task, string conclusion)
        {
            if (!_quests.ContainsKey(questId))
                Open(questId, title);
            else
                _journalQuests.Log(questId, _quests[questId].Conclusion);
            
            _quests[questId].Update(title, task, conclusion);
            
            OnUpdate?.Invoke(questId, title, task);
        }

        public void Close(int questId, string result)
        {
            _quests[questId].Finish();
            _journalQuests.Close(questId, result);
            
            OnClose?.Invoke(questId);
        }
        
        private void Open(int questId, string title)
        {
            _quests.Add(questId, new Quest(questId));
            _journalQuests.Open(questId, title);
            
            OnOpen?.Invoke(questId);
        }
    }
}