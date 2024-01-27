using System;
using UnityEngine;

namespace PointNClick.Movement
{
    public interface IMover
    {
        public float Speed { get; }

        public event Action OnArrived;
        
        public void Move(Vector3 target);
        public void Stop();
    }
}