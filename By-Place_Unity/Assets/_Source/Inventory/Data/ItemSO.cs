using UnityEngine;

namespace Inventory.Data
{
    [CreateAssetMenu(menuName = "SO/Inventory/ItemSO", fileName = "NewItemSO")]
    public class ItemSO : ScriptableObject
    {
        public int Id => GetInstanceID();

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField, TextArea] public string Description { get; private set; }

        public Item Build()
        {
            var item = new Item(name, Id, Name, Description);
            item.SetIcon(Icon);
            
            return item;
        }
    }
}