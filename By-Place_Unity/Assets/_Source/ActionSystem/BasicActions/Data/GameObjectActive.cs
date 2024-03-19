using System;
using UnityEngine;

namespace ActionSystem.BasicActions.Data
{
    [Serializable]
    public class GameObjectActive
    { 
        [field: SerializeField] public GameObject GameObject { get; private set; }
        [field: SerializeField] public bool Active { get; private set; }
    }
}