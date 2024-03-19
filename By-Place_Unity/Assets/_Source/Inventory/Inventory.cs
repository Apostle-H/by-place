using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Data;
using SaveLoad;
using SaveLoad.Invoker;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Inventory
{
    public class Inventory : ISavableLoadable<Dictionary<int, string>>, IStartable, IDisposable
    {
        private ISaverLoader _saverLoader;
        private SaveLoadInvoker _saveLoadInvoker;

        private Dictionary<int, Item> _items = new();
        
        public string Path => "Inventory";

        public event Action<Item> OnAddItem;
        public event Action<Item> OnRemoveItem;

        [Inject]
        public Inventory(ISaverLoader saverLoader, SaveLoadInvoker saveLoadInvoker)
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

        public void AddItem(Item item)
        {
            if (_items.ContainsKey(item.Id))
                return;
            
            _items.Add(item.Id, item);
            
            OnAddItem?.Invoke(item);
        }

        public void RemoveItem(int itemId)
        {
            if (!_items.ContainsKey(itemId))
                return;

            var item = _items[itemId];
            _items.Remove(itemId);
            
            OnRemoveItem?.Invoke(item);
        }

        private void Save() => _saverLoader.Save(this);

        private void Load() => _saverLoader.Load(this);
        
        public Dictionary<int, string> GetSaveData() => 
            _items.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AssetName);

        public void LoadSaveData(Dictionary<int, string> saveData)
        {
            foreach (var kvp in saveData)
            {
                var itemSO = Resources.Load<ItemSO>($"Items/{kvp.Value}");
                AddItem(itemSO.Build());
            }
        }
    }
}