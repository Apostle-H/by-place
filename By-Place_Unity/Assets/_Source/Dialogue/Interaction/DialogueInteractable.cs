using System;
using Dialogue.Data.Save;
using Dialogue.Resolve;
using Interactions;
using UnityEngine;
using VContainer;

namespace Dialogue.Interaction
{
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DGroupSO ds;
        [SerializeField] private Transform interactPoint;

        private DialogueController _dialogueController;
        
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