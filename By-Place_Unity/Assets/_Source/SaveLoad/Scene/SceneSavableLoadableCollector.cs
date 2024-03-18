using System;
using System.Collections.Generic;
using Registration;
using SaveLoad.Invoker;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SaveLoad.Scene
{
    public class SceneSavableLoadableCollector : IRegistrator<SceneSavableLoadable>, IStartable, IDisposable
    {
        private ISaverLoader _saverLoader;
        private SaveLoadInvoker _saveLoadInvoker;
        
        private Dictionary<int, SceneSavableLoadable> _savableLoadables = new();

        [Inject]
        public SceneSavableLoadableCollector(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
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

        public void Register(SceneSavableLoadable registratable)
        {
            if (_savableLoadables.ContainsKey(registratable.GetInstanceID()))
                return;
            
            _savableLoadables.Add(registratable.GetInstanceID(), registratable);
        }

        public void Unregister(SceneSavableLoadable registratable)
        {
            if (!_savableLoadables.ContainsKey(registratable.GetInstanceID()))
                return;
            
            _savableLoadables.Remove(registratable.GetInstanceID());
        }

        private void Save()
        {
            foreach (var kvp in _savableLoadables)
                _saverLoader.Save(kvp.Value);
        }

        private void Load()
        {
            foreach (var kvp in _savableLoadables)
                _saverLoader.Load(kvp.Value);
        }
    }
}