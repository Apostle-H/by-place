using System;
using ActionSystem;
using ActionSystem.Data;
using Movement.Action.Data;
using UnityEngine;
using VContainer;

namespace Movement.Action
{
    public class MoveAction : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private MovePath movePath;

        private ActionResolver _actionResolver;

        private int _arrivedCount;
        
        public int Id => actionSO.Id;
        public bool Resolve { get; private set; } = true;
        
        public event Action<IAction> OnFinished;
        
        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        public void Start() => _actionResolver.AddAction(this);

        public void OnDestroy() => _actionResolver.RemoveAction(this);
        
        public void Perform()
        {
            movePath.mover.OnArrived += Finished;
            movePath.mover.Move(movePath.target.position);
            
            Resolve = false;
        }

        private void Finished()
        {
            movePath.mover.OnArrived -= Finished;

            OnFinished?.Invoke(this);
        }
    }
}