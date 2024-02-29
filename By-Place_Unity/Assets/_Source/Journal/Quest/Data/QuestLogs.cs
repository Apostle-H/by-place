using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Journal.Quest.Data
{
    public class QuestLogs
    {
        private List<string> _logs = new();

        public int QuestId { get; set; }
        public ReadOnlyCollection<string> Logs => _logs.AsReadOnly();
        public bool Opened { get; private set; }

        public QuestLogs(int questId)
        {
            QuestId = questId;
            
            Opened = true;
        }

        public void AddLog(string log) => _logs.Add(log);

        public void Close() => Opened = false;
    }
}