using System;
using ActionSystem;
using ActionSystem.Data;
using UnityEngine;
using VContainer;

namespace Navigation.TravelPoint
{
    public class TravelPointLock : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private TravelPoint travelPoint;

        [SerializeField] private bool isOpen;

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
            if (isOpen)
                travelPoint.Unlock();
            else
                travelPoint.Lock();
            
            Resolvable = false;
            
            OnFinished?.Invoke(this);
        }
    }
}