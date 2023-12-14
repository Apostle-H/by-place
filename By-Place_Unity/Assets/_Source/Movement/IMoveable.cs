using UnityEngine;

namespace Movement
{
    public interface IMoveable
    {
        public float Speed { get; }
        
        public void Move(Vector3 target);
        public void Stop();
    }
}