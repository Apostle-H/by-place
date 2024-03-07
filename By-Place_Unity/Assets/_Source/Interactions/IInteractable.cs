using System;
using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        public Vector3 Position { get; }

        public event Action OnStarted;
        public event Action OnFinished;

        public void Interact();
    }
}