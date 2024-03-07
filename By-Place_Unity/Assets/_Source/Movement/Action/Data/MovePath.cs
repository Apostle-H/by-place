using System;
using UnityEngine;

namespace Movement.Action.Data
{
    [Serializable]
    public class MovePath
    {
        [field: SerializeField] public AMover mover;
        [field: SerializeField] public Transform target;
    }
}