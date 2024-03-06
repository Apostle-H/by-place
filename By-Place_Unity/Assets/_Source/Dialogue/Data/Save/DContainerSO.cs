using System;
using System.Collections.Generic;
using Dialogue.Data.Save.Nodes;
using UnityEditor.Callbacks;
using UnityEngine;
using Utils.Services;
#if UNITY_EDITOR
#endif

namespace Dialogue.Data.Save
{
    public partial class DContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DGroupSO> Groups { get; private set; } = new();
        [field: SerializeField] public List<DNodeSO> Nodes { get; private set; } = new();

        public static event Action<DContainerSO> OnOpen;

#if UNITY_EDITOR
        [OnOpenAsset]
        private static bool Open(int instanceID, int line)
        {
            var asset = AssetsService.LoadAsset<DContainerSO>(instanceID);
            if (asset == default)
                return false;

            OnOpen?.Invoke(asset);
            return true;
        }
#endif
    }
}