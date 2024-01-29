using System;
using UnityEngine;

namespace PointNClick.Movement
{
    public interface IMover
    {
        public float Speed { get; }
        public float CurrentSpeed { get; }

        public event Action OnArrived;
        public event Action<float> OnSpeedUpdate;
        
        public void Move(Vector3 target);
        public void Stop();
    }
}