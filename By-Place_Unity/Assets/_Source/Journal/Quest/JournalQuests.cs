using System;
using System.Collections.Generic;
using Journal.Quest.Data;
using SaveLoad;
using SaveLoad.Invoker;
using VContainer;
using VContainer.Unity;

namespace Journal.Quest
{
    public class JournalQuests : ISavableLoadable<Dictionary<int, QuestLogs>>, IStartable, IDisposable
    {
        private readonly ISaverLoader _saverLoader;
        private readonly SaveLoadInvoker _saveLoadInvoker;
        
        private Dictionary<int, QuestLogs> _logs = new();

        public string Path => "JournalQuests";
        
        public event Action<int, string> OnQuestOpened; 
        public event Action<int, string> OnLogAdded;
        public event Action<int, string> OnQuestClosed;

        [Inject]
        private JournalQuests(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
        {
            _saverLoader = saverLoader;
            _saveLoadInvoker = saveLoadInvoker;
        }
        
        public void Start() => Bind();

        public void Dispose() => Expose();

        private void Bind()
        {
            _saveLoadInvoker.OnSave += Save;
            _saveLoadInvoker.OnLoad += Load;
        }

        private void Expose()
        {
            _saveLoadInvoker.OnSave -= Save;
            _saveLoadInvoker.OnLoad -= Load;
        }
        
        public void Open(int questId, string title)
        {
            _logs.Add(questId, new QuestLogs(questId, title));

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

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);

        public Dictionary<int, QuestLogs> GetSaveData() => new(_logs);

        public void LoadSaveData(Dictionary<int, QuestLogs> saveData)
        {
            _logs.Clear();
            foreach (var kvp in saveData)
            {
                Open(kvp.Key, kvp.Value.Title);
                foreach (var log in kvp.Value.Logs)
                {
                    Log(kvp.Key, log);
                }
            }
        }
    }
}