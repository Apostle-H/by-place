using System;
using DialogueSystem.ActionSystem;
using DialogueSystem.ActionSystem.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Navigation.TravelPoint
{
    public class TravelPointLock : MonoBehaviour, IAction
    {
        [SerializeField] private ActionSO actionSO;
        [SerializeField] private TravelPoint travelPoint;

        [SerializeField] private bool lockUnlock;

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
            if (lockUnlock)
                travelPoint.Unlock();
            else
                travelPoint.Lock();
            
            Resolve = false;
            
            OnFinished?.Invoke(this);
        }
    }
}