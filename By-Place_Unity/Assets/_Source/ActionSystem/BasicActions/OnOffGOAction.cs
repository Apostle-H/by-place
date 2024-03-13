using System;
using ActionSystem.Data;
using UnityEngine;
using VContainer;

namespace ActionSystem.BasicActions
{
    public class OnOffGOAction : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private GameObject[] targets;
        [SerializeField] private bool offOn;

        private ActionResolver _actionResolver;

        public int Id => actionSO.Id;
        public bool Resolvable { get; private set; } = true;
        
        public event Action<IAction> OnFinished;

        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        public void Start() => _actionResolver.Register(this);

        public void OnDestroy() => _actionResolver.Unregister(this);

        public void Resolve()
        {
            foreach (var target in targets)
                target.SetActive(offOn);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }

        public void Skip() { }
    }
}