using System;
using ActionSystem.BasicActions.Data;
using ActionSystem.Data;
using UnityEngine;
using VContainer;

namespace ActionSystem.BasicActions
{
    public class OnOffGOAction : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private GameObjectActive[] targets;

        private ActionResolver _actionResolver;

        public int Id => actionSO.Id;
        public bool Resolvable { get; set; } = true;
        
        public event Action<IAction> OnFinished;

        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        public void Start() => _actionResolver.Register(this);

        public void OnDestroy() => _actionResolver.Unregister(this);

        public void Resolve()
        {
            foreach (var target in targets)
                target.GameObject.SetActive(target.Active);
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }

        public void Skip() { }
    }
}