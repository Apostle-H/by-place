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
        public bool Resolvable { get; private set; } = true;
        
        public event Action<IAction> OnFinished;
        
        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        public void Start() => _actionResolver.Register(this);

        public void OnDestroy() => _actionResolver.Unregister(this);
        
        public void Resolve()
        {
            movePath.mover.OnStopped += Finished;
            movePath.mover.Move(movePath.target.position);
            
            Resolvable = false;
        }

        private void Finished(bool result)
        {
            movePath.mover.OnStopped -= Finished;
            if (!result)
                return;
            
            movePath.mover.Rotate(movePath.target.rotation);
            
            OnFinished?.Invoke(this);
        }
    }
}