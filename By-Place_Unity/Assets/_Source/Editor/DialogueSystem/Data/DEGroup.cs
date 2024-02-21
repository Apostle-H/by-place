using System;
using DialogueSystem.Elements;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DEGroup
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
        
        [field: SerializeField] public int RuntimeAssetId { get; private set; }

        public void Save(DGGroup group)
        {
            Guid = group.Guid;
            Name = group.title;
            Position = group.GetPosition().position;

            RuntimeAssetId = group.RuntimeAssetId;
        }

        public DGGroup Load()
        {
            var group = new DGGroup(Name, Position, Guid, RuntimeAssetId);
            return group;
        }
    }
}