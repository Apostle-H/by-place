using Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory.View.Data
{
    [CreateAssetMenu(menuName = "SO/Inventory/View/Config", fileName = "NewInventoryViewConfigSO")]
    public class InventoryViewConfigSO : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset ItemSlotVisualTree { get; private set; }
        [field: SerializeField] public CursorConfigSO ToggleCursorConfigSO { get; private set; }
        [field: SerializeField] public Sprite ClosedInventorySprite { get; private set; }
        [field: SerializeField] public Sprite OpenedInventorySprite { get; private set; }
    }
}