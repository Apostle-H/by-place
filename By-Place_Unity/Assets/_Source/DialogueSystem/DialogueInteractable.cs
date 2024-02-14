using System;
using Character.View;
using DialogueSystem;
using DialogueSystem.Data.Save;
using UnityEngine;
using VContainer;

namespace PointNClick.Interactions
{
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DRGroupSO ds;
        [SerializeField] private Transform interactPoint;

        private DialogueResolver _dialogueResolver;
        
        private CharacterAnimationParams _characterAnimationParams;
        
        public Vector3 Position => interactPoint.position;
        
        public event Action OnStarted;
        public event Action OnFinished;

        [Inject]
        public void Inject(DialogueResolver dialogueResolver) => _dialogueResolver = dialogueResolver;

        public void Interact()
        {
            _dialogueResolver.Load(ds);
            _dialogueResolver.OnQuit += Finished;
            OnStarted?.Invoke();
        }

        private void Finished()
        {
            _dialogueResolver.OnQuit -= Finished;
            OnFinished?.Invoke();
        }
    }
}