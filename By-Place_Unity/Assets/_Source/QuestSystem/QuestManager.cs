using System;
using System.Collections.Generic;
using QuestSystem.Data;
using SaveLoad;
using SaveLoad.Invoker;
using VContainer;
using VContainer.Unity;

namespace QuestSystem
{
    public class QuestManager : ISavableLoadable<Dictionary<int, Quest>>, IStartable, IDisposable
    {
        private readonly ISaverLoader _saverLoader;
        private readonly SaveLoadInvoker _saveLoadInvoker;

        private Dictionary<int, Quest> _quests = new();

        public string Path => "Quests";
        
        public event Action<Quest> OnOpen;
        public event Action<Quest> OnUpdate;
        public event Action<Quest> OnClose;

        [Inject]
        public QuestManager(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
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

        public void Open(int questId, string title, string task, string conclusion)
        {
            _quests.Add(questId, new Quest(questId, title, task, conclusion));
            
            OnOpen?.Invoke(_quests[questId]);
        }
        
        public void Update(int questId, string task, string conclusion)
        {
            _quests[questId].Update(task, conclusion);
            
            OnUpdate?.Invoke(_quests[questId]);
        }

        public void Close(int questId, string result)
        {
            var quest = _quests[questId];
            quest.Close(result);
            _quests.Remove(questId);
            
            OnClose?.Invoke(quest);
        }

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);
        
        public Dictionary<int, Quest> GetSaveData() => new(_quests);

        public void LoadSaveData(Dictionary<int, Quest> saveData)
        {
            _quests.Clear();
            foreach (var kvp in saveData)
            {
                var quest = kvp.Value;
                Open(quest.Id, quest.Title, quest.Task, quest.NextConclusion);
                foreach (var conclusion in quest.Conclusions)
                    Update(quest.Id, quest.Task, conclusion);
            }
        }
    }
}