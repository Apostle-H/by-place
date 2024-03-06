using System;
using UnityEngine;

namespace Dialogue.Data.NodeParams
{
    [Serializable]
    public class DOutputData
    {
        [field: SerializeField] public int NextGuid { get; set; } = -1;
    }
}