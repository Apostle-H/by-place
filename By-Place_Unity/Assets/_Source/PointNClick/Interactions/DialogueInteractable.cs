using System;
using DialogueSystem.Data.Save;
using DialogueSystem.View;
using UnityEngine;
using VContainer;

namespace PointNClick.Interactions
{
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueGroupSO dialogue;
        [SerializeField] private Transform speakPoint;

        private DialogueResolver _dialogueResolver;
        
        public Vector3 Position => speakPoint.position;
        public bool BlockMovement => true;
        
        public event Action OnFinished;

        [Inject]
        public void Init(DialogueResolver dialogueResolver) => _dialogueResolver = dialogueResolver;

        public void Interact()
        {
            _dialogueResolver.Load(dialogue);
            _dialogueResolver.OnQuit += Finished;
        }

        private void Finished()
        {
            _dialogueResolver.OnQuit -= Finished;
            OnFinished?.Invoke();
        }
    }
}