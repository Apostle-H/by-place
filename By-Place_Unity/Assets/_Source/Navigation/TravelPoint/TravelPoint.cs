using System;
using Character.Movement;
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

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        
        public event Action OnStarted;
        public event Action<bool> OnFinished;
        
        [Inject]
        private void Inject(ICharacterMover characterMover) => _characterMover = characterMover;
        
        public void Lock() => isOpen = false;

        public void Unlock() => isOpen = true;

        public void Interact()
        {
            if (!isOpen)
            {
                OnFinished?.Invoke(false);
                return;
            }

            fromLocation.Exit();
            toLocation.Enter();
            
            _characterMover.Move(targetPos.position);
            _characterMover.OnStopped += Finished;

            OnStarted?.Invoke();
        }

        private void Finished(bool result)
        {
            _characterMover.OnStopped -= Finished;
            OnFinished?.Invoke(result);
        }
    }
}