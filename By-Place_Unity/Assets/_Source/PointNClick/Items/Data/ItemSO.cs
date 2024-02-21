using UnityEngine;

namespace PointNClick.Items.Data
{
    [CreateAssetMenu(menuName = "SO/Inventory/ItemSO", fileName = "NewItemSO")]
    public class ItemSO : ScriptableObject
    {
        public int Id => GetInstanceID();

        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}