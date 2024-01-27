using System;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DOutputData
    {
        [field: SerializeField] public int NextGuid { get; set; } = -1;
    }
}