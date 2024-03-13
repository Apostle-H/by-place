using System;
using Identity.Data;
using Registration;
using UnityEngine;

namespace Navigation.Location
{
    public abstract class ALocation : MonoBehaviour, IIdentity
    {
        public abstract int Id { get; }
        public abstract event Action OnEnter;
        public abstract event Action OnExit;
        
        public abstract void Enter();
        public abstract void Exit();
    }
}