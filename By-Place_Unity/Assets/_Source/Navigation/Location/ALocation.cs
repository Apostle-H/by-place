using UnityEngine;

namespace Navigation.Location
{
    public abstract class ALocation : MonoBehaviour
    {
        public abstract void Enter();
        public abstract void Exit();
    }
}