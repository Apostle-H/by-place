using System;
using UnityEngine;

namespace Movement
{
    public interface IMover
    {
        public float Speed { get; }
        public float CurrentSpeed { get; }

        public event System.Action OnDeparted;
        public event Action<bool> OnStopped;
        public event Action<float> OnSpeedUpdate;
        
        public void Move(Vector3 target);
        public void Rotate(Quaternion rotation);
        public void Stop();
    }
}