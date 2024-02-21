using System;
using Character.View;
using DialogueSystem.Data.Save;
using PointNClick.Interactions;
using UnityEngine;
using VContainer;

namespace DialogueSystem
{
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DRGroupSO ds;
        [SerializeField] private Transform interactPoint;

        private DialogueController _dialogueController;
        
        private CharacterAnimationParams _characterAnimationParams;
        
        public Vector3 Position => interactPoint.position;
        
        public event Action OnStarted;
        public event Action OnFinished;

        [Inject]
        public void Inject(DialogueController dialogueController) => _dialogueController = dialogueController;

        public void Interact()
        {
            _dialogueController.Load(ds);
            _dialogueController.OnQuit += Finished;
            OnStarted?.Invoke();
        }

        private void Finished()
        {
            _dialogueController.OnQuit -= Finished;
            OnFinished?.Invoke();
        }
    }
}