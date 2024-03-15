﻿using System;
using ActionSystem;
using Inventory.Data;
using Inventory.View;

namespace Inventory.Actions
{
    public class ItemAction : IAction
    {
        private InventoryView _inventoryView;
        
        private ItemSO _itemSO;
        private bool _removeAdd;

        public int Id { get; private set; }
        public bool Resolvable { get; set; } = true;
        
        public event Action<IAction> OnFinished;

        public ItemAction(int id, ItemSO itemSO, bool removeAdd, InventoryView inventoryView)
        {
            Id = id;

            _itemSO = itemSO;
            _removeAdd = removeAdd;
            _inventoryView = inventoryView;
        }

        public void Resolve()
        {
            if (_removeAdd)
                _inventoryView.AddItem(_itemSO);
            else
                _inventoryView.RemoveItem(_itemSO);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }

        public void Skip() { }
    }
}