using System;
using System.Collections.Generic;
using Journal.Quest.Data;

namespace Journal.Quest
{
    public class JournalQuests
    {
        private Dictionary<int, QuestLogs> _logs = new();

        public event Action<int, string> OnQuestOpened; 
        public event Action<int, string> OnLogAdded;
        public event Action<int, string> OnQuestClosed;
        
        public void Open(int questId, string title)
        {
            _logs.Add(questId, new QuestLogs(questId));

            OnQuestOpened?.Invoke(questId, title);
        }
        
        public void Log(int questId, string log)
        {
            _logs[questId].AddLog(log);
            OnLogAdded?.Invoke(questId, log);
        }

        public void Close(int questId, string result)
        {
            _logs[questId].Close();
            
            OnQuestClosed?.Invoke(questId, result);
        }
    }
}