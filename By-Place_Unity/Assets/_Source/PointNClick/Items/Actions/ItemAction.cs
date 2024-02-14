using System;
using DialogueSystem.ActionSystem;
using PointNClick.Items.Actions.Data;
using PointNClick.Items.View;
using UnityEngine;
using VContainer;

namespace PointNClick.Items.Actions
{
    public class ItemAction : IAction
    {
        private InventoryView _inventoryView;
        
        private ItemSO _itemSO;
        private bool _removeAdd;

        public int Id { get; private set; }
        public bool Resolve { get; private set; } = true;
        
        public event Action<IAction> OnFinished;

        public ItemAction(int id, ItemSO itemSO, bool removeAdd, InventoryView inventoryView)
        {
            Id = id;

            _itemSO = itemSO;
            _removeAdd = removeAdd;
            _inventoryView = inventoryView;
        }

        public void Perform()
        {
            if (_removeAdd)
                _inventoryView.AddItem(_itemSO);
            else
                _inventoryView.RemoveItem(_itemSO);
            Resolve = false;
            
            OnFinished?.Invoke(this);
        }
    }
}