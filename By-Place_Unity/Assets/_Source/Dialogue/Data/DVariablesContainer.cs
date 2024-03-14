using System;
using System.Collections.Generic;
using SaveLoad;
using SaveLoad.Invoker;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Dialogue.Data
{
    public class DVariablesContainer : ISavableLoadable<Dictionary<int, bool>>, IStartable, IDisposable
    {
        private ISaverLoader _saverLoader;
        private SaveLoadInvoker _saveLoadInvoker;
        
        private Dictionary<int, bool> _variables = new();
        
        public string Path => "Variables";
        
        [Inject]
        public DVariablesContainer(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
        {
            _saverLoader = saverLoader;
            _saveLoadInvoker = saveLoadInvoker;
        }
        
        public void Start()
        {
            LoadDefault();
            
            Bind();
        }

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

        public bool Get(int id, out bool variable)
        {
            variable = false;
            if (!_variables.ContainsKey(id))
                return false;

            variable = _variables[id];
            return true;
        }

        public bool Set(int id, bool value = true)
        {
            if (!_variables.ContainsKey(id))
                return false;

            _variables[id] = value;
            return true;
        }

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);
        
        public Dictionary<int, bool> GetSaveData() => new(_variables);

        public void LoadSaveData(Dictionary<int, bool> saveData)
        {
            _variables.Clear();
            foreach (var kvp in saveData)
                _variables.Add(kvp.Key, kvp.Value);
        }

        private void LoadDefault()
        {
            var variablesSOs = Resources.LoadAll<DVariableSO>("Variables");
            foreach (var variable in variablesSOs)
                _variables.Add(variable.Id, variable.Value);

            foreach (var variableSO in variablesSOs)
                Resources.UnloadAsset(variableSO);
        }
    }
}