using System;
using Character.Data;
using Interactions;
using Movement;
using Navigation.Location;
using UnityEngine;
using VContainer;

namespace Navigation.TravelPoint
{
    public class TravelPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private ALocation fromLocation;
        [SerializeField] private ALocation toLocation;
        
        [SerializeField] private Transform targetPos;
        [SerializeField] private bool isOpen;

        private ICharacterMover _characterMover;

        [Inject]
        private void Inject(ICharacterMover characterMover) => _characterMover = characterMover;

        public Vector3 Position => transform.position;
        
        public event Action OnStarted;
        public event Action OnFinished;
        
        public void Lock() => isOpen = false;

        public void Unlock() => isOpen = true;

        public void Interact()
        {
            if (!isOpen)
            {
                OnFinished?.Invoke();
                return;
            }

            fromLocation.Exit();
            toLocation.Enter();
            
            _characterMover.Move(targetPos.position);
            _characterMover.OnArrived += Finished;

            OnStarted?.Invoke();
        }

        private void Finished()
        {
            _characterMover.OnArrived -= Finished;
            OnFinished?.Invoke();
        }
    }
}