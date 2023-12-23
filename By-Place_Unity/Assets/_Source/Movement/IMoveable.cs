using System;
using UnityEngine;

namespace Movement
{
    public interface IMoveable
    {
        public float Speed { get; }

        public event Action OnArrived;
        
        public void Move(Vector3 target);
        public void Stop();
    }
}