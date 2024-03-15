using System;
using UnityEngine;
using VContainer;

namespace Navigation.Location
{
    public class BasicLocation : ALocation
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform cameraPosition;

        private LocationsProvider _locationsProvider;
        
        public override int Id => GetHashCode();
        
        public override event Action OnEnter;
        public override event Action OnExit;

        [Inject]
        private void Inject(LocationsProvider locationsProvider) => _locationsProvider = locationsProvider;

        private void Start() => _locationsProvider.Register(this);

        private void OnDestroy() => _locationsProvider.Unregister(this);

        public override void Enter()
        {
            targetCamera.transform.position = cameraPosition.position;
            targetCamera.transform.rotation = cameraPosition.rotation;
            
            OnEnter?.Invoke();
        }

        public override void Exit()
        {
            OnExit?.Invoke();
        }
    }
}