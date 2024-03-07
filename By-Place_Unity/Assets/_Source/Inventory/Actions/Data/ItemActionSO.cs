﻿using ActionSystem.Data;
using Inventory.Data;
using UnityEngine;

namespace Inventory.Actions.Data
{
    [CreateAssetMenu(menuName = "SO/Inventory/ItemActionSO", fileName = "NewItemActionSO")]
    public class ItemActionSO : ActionSO
    {
        [field: SerializeField] public ItemSO ItemSO { get; private set; }
        [field: SerializeField] public bool RemoveAdd { get; private set; }
    }
}