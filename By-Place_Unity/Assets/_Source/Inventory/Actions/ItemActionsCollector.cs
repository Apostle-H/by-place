using System;
using System.Collections.Generic;
using System.Linq;
using ActionSystem;
using Inventory.Actions.Data;
using Inventory.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Inventory.Actions
{
    public class ItemActionsCollector : IInitializable, IStartable, IDisposable
    {
        private List<ItemAction> _itemActions = new();

        private Inventory _inventory;
        private ActionResolver _actionResolver;


        [Inject]
        private void Inject(Inventory inventory, ActionResolver actionResolver)
        {
            _inventory = inventory;
            _actionResolver = actionResolver;
        }

        public void Initialize()
        {
            var questActionsSO = Resources.LoadAll<ItemActionSO>("Actions/Inventory").ToList();

            foreach (var itemActionSO in questActionsSO)
                _itemActions.Add(new ItemAction(itemActionSO.Id, itemActionSO.ItemSO, itemActionSO.RemoveAdd, _inventory));
        }

        public void Start()
        {
            foreach (var itemAction in _itemActions)
                _actionResolver.Register(itemAction);
        }

        public void Dispose()
        {
            foreach (var itemAction in _itemActions)
                _actionResolver.Unregister(itemAction);
        }
    }
}