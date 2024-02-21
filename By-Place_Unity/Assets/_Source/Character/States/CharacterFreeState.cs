using Character.Data;
using InputSystem;
using PointNClick.Data;
using PointNClick.Interactions;
using StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils.Extensions;
using Utils.Services;
using VContainer;

namespace Character.States
{
    public class CharacterFreeState : IState
    {
        private PointNClickConfigSO _configSO;
        
        private PointNClickActions _actions;
        private CharacterComponents _characterComponents;

        public IStateMachine Owner { get; set; }

        [Inject]
        public CharacterFreeState(PointNClickConfigSO configSO, PointNClickActions actions,
            CharacterComponents characterComponents)
        {
            _configSO = configSO;
            _actions = actions;
            _characterComponents = characterComponents;
        }
        
        public void Enter() => Bind();

        public void Update() { }

        public void Exit() => Expose();

        private void Bind()
        {
            _actions.Main.Interact.canceled += Click;
            _actions.Main.Stop.performed += Cancel;
        }

        private void Expose()
        {
            _actions.Main.Interact.canceled -= Click;
            _actions.Main.Stop.performed -= Cancel;
        }

        private void Click(InputAction.CallbackContext ctx)
        {
            var screenPoint = _actions.Main.Point.ReadValue<Vector2>();
            
            PhysicsService.RayCastFromCamera(screenPoint, out var hit);
            if (hit.collider == default)
                return;
            
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                _characterComponents.TargetInteractable = interactable;
                Owner.Switch<CharacterInteractingState>();
                return;
            }
            
            if (_configSO.WalkableMask.Contains(hit.collider.gameObject.layer))
                Move(hit.point);
        }

        private void Cancel(InputAction.CallbackContext ctx) => _characterComponents.Mover.Stop();

        private void Move(Vector3 target) => _characterComponents.Mover.Move(target);

        
    }
}