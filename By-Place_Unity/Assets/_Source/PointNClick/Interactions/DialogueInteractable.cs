using System;
using DialogueSystem;
using DialogueSystem.Data.Save;
using UnityEngine;
using VContainer;

namespace PointNClick.Interactions
{
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DRGroupSO ds;
        [SerializeField] private Transform speakPoint;

        private DialogueResolver _dialogueResolver;
        
        public Vector3 Position => speakPoint.position;
        
        public event Action OnFinished;

        [Inject]
        public void Inject(DialogueResolver dialogueResolver) => _dialogueResolver = dialogueResolver;

        public void Interact()
        {
            _dialogueResolver.Load(ds);
            _dialogueResolver.OnQuit += Finished;
        }

        private void Finished()
        {
            _dialogueResolver.OnQuit -= Finished;
            OnFinished?.Invoke();
        }
    }
}