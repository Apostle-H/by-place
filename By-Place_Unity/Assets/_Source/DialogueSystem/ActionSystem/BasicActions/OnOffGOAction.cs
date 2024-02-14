using System;
using DialogueSystem.ActionSystem.Data;
using UnityEngine;
using VContainer;

namespace DialogueSystem.ActionSystem.BasicActions
{
    public class OnOffGOAction : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private GameObject[] targets;
        [SerializeField] private bool offOn;

        private ActionResolver _actionResolver;

        public int Id => actionSO.Id;
        public bool Resolve { get; private set; } = true;
        
        public event Action<IAction> OnFinished;

        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        public void Start() => _actionResolver.AddAction(this);

        public void OnDestroy() => _actionResolver.RemoveAction(this);

        public void Perform()
        {
            foreach (var target in targets)
                target.SetActive(offOn);
            Resolve = false;
            
            OnFinished?.Invoke(this);
        }
    }
}