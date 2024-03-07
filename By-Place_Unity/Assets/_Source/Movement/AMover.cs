using System;
using UnityEngine;

namespace Movement
{
    public abstract class AMover : MonoBehaviour, IMover
    {
        public abstract float Speed { get; }
        public abstract float CurrentSpeed { get; protected set; }
        public abstract event System.Action OnArrived;
        public abstract event Action<float> OnSpeedUpdate;

        public abstract void Move(Vector3 target);
        public abstract void Rotate(Quaternion rotation);

        public abstract void Stop();
    }
}