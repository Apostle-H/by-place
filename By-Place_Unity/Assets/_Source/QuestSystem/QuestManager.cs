using System;
using System.Collections.Generic;
using Journal.Quest;
using QuestSystem.Data;
using SaveLoad;
using SaveLoad.Invoker;
using VContainer;
using VContainer.Unity;

namespace QuestSystem
{
    public class QuestManager : ISavableLoadable<Dictionary<int, Quest>>, IStartable, IDisposable
    {
        private JournalQuests _journalQuests;
        private readonly ISaverLoader _saverLoader;
        private readonly SaveLoadInvoker _saveLoadInvoker;

        private Dictionary<int, Quest> _quests = new();

        public string Path => "Quests";
        
        public event Action<int> OnOpen;
        /// <summary>
        /// -questId, title, task-
        /// </summary>
        public event Action<int, string, string> OnUpdate;
        public event Action<int> OnClose;

        [Inject]
        public QuestManager(JournalQuests journalQuests, ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
        {
            _journalQuests = journalQuests;
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
            _quests.Remove(questId);
            _journalQuests.Close(questId, result);
            
            OnClose?.Invoke(questId);
        }
        
        private void Open(int questId, string title)
        {
            _quests.Add(questId, new Quest(questId));
            _journalQuests.Open(questId, title);
            
            OnOpen?.Invoke(questId);
        }

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);
        
        public Dictionary<int, Quest> GetSaveData() => new(_quests);

        public void LoadSaveData(Dictionary<int, Quest> saveData)
        {
            _quests.Clear();
            foreach (var kvp in saveData)
                Update(kvp.Key, kvp.Value.Title, kvp.Value.Task, kvp.Value.Conclusion);
        }
    }
}