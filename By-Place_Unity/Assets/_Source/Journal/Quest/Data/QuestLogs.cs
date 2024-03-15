using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Journal.Quest.Data
{
    public class QuestLogs
    {
        private List<string> _logs = new();

        public int QuestId { get; private set; }
        public string Title { get; private set; }
        
        public ReadOnlyCollection<string> Logs => _logs.AsReadOnly();
        public bool Opened { get; private set; }

        public QuestLogs(int questId, string title)
        {
            QuestId = questId;
            Title = title;
            
            Opened = true;
        }

        [JsonConstructor]
        private QuestLogs(int questId, string title, List<string> logs, bool opened)
        {
            QuestId = questId;
            Title = title;
            _logs = logs;
            Opened = opened;
        }

        public void AddLog(string log) => _logs.Add(log);

        public void Close() => Opened = false;
    }
}