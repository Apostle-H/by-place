using System;
using System.Collections.Generic;
using System.Linq;
using ActionSystem;
using PointNClick.Inventory.Actions.Data;
using PointNClick.Inventory.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Inventory.Actions
{
    public class ItemActionsCollector : IInitializable, IStartable, IDisposable
    {
        private List<ItemAction> _itemActions = new();

        private InventoryView _inventoryView;
        private ActionResolver _actionResolver;


        [Inject]
        private void Inject(InventoryView inventoryView, ActionResolver actionResolver)
        {
            _inventoryView = inventoryView;
            _actionResolver = actionResolver;
        }

        public void Initialize()
        {
            var questActionsSO = Resources.LoadAll<ItemActionSO>("Actions/Inventory").ToList();

            foreach (var itemActionSO in questActionsSO)
                _itemActions.Add(new ItemAction(itemActionSO.Id, itemActionSO.ItemSO, itemActionSO.RemoveAdd, 
                    _inventoryView));
        }

        public void Start()
        {
            foreach (var itemAction in _itemActions)
                _actionResolver.AddAction(itemAction);
        }

        public void Dispose()
        {
            foreach (var itemAction in _itemActions)
                _actionResolver.RemoveAction(itemAction);
        }
    }
}