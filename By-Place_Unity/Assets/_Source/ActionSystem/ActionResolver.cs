using System;
using System.Collections.Generic;
using System.Linq;
using Registration;
using SaveLoad;
using SaveLoad.Invoker;
using VContainer;
using VContainer.Unity;

namespace ActionSystem
{
    public class ActionResolver : IRegistrator<IAction>, ISavableLoadable<Dictionary<int, bool>>, IStartable, IDisposable
    {
        private ISaverLoader _saverLoader;
        private SaveLoadInvoker _saveLoadInvoker;
        
        private Dictionary<int, IAction> _actions = new();

        public string Path => "Actions";

        public event Action OnFinished;

        [Inject]
        public ActionResolver(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
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
        
        public void Register(IAction action)
        {
            if (_actions.ContainsKey(action.Id))
                return;
            
            _actions.Add(action.Id, action);
        }

        public void Unregister(IAction action) => _actions.Remove(action.Id);

        public void Resolve(int id)
        {
            if (!_actions.ContainsKey(id) || !_actions[id].Resolvable)
            {
                OnFinished?.Invoke();
                return;
            }

            _actions[id].OnFinished += Finished;
            _actions[id].Resolve();
        }

        private void Finished(IAction action)
        {
            action.OnFinished -= Finished;
            OnFinished?.Invoke();
        }

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);

        public Dictionary<int, bool> GetSaveData() => 
            _actions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Resolvable);

        public void LoadSaveData(Dictionary<int, bool> saveData)
        {
            foreach (var kvp in saveData)
            {
                if (!_actions.ContainsKey(kvp.Key))
                    return;

                _actions[kvp.Key].Resolvable = kvp.Value;
            }
        }
    }
}