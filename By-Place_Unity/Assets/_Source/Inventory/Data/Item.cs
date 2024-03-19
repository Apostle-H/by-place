using Newtonsoft.Json;
using UnityEngine;

namespace Inventory.Data
{
    public class Item
    {
        public int Id { get; private set; }

        public string AssetName { get; private set; }
        public string Name { get; private set; }
        [JsonIgnore] public Sprite Icon { get; private set; }
        public string Description { get; private set; }

        public Item(string assetName, int id, string name, string description)
        {
            AssetName = assetName; 
            Id = id;
            Name = name;
            Description = description;
        }

        public void SetIcon(Sprite icon) => Icon = icon;
    }
}