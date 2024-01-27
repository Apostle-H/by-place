using System;
using UnityEngine;

namespace PointNClick.Interactions
{
    public interface IInteractable
    {
        public Vector3 Position { get; }

        public event Action OnFinished;

        public void Interact();
    }
}