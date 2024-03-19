using System;
using ActionSystem;
using Inventory.Data;
using Inventory.View;

namespace Inventory.Actions
{
    public class ItemAction : IAction
    {
        private Inventory _inventory;
        
        private ItemSO _itemSO;
        private bool _removeAdd;

        public int Id { get; private set; }
        public bool Resolvable { get; set; } = true;
        
        public event Action<IAction> OnFinished;

        public ItemAction(int id, ItemSO itemSO, bool removeAdd, Inventory inventory)
        {
            Id = id;

            _itemSO = itemSO;
            _removeAdd = removeAdd;
            _inventory = inventory;
        }

        public void Resolve()
        {
            if (_removeAdd)
                _inventory.AddItem(_itemSO.Build());
            else
                _inventory.RemoveItem(_itemSO.Id);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }

        public void Skip() { }
    }
}