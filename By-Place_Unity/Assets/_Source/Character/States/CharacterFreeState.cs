using InputSystem;
using PointNClick.Data;
using PointNClick.Interactions;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Services;
using VContainer;

namespace Character.States
{
    public class CharacterFreeState : IState
    {
        private PointNClickActions _actions;
        private CharacterComponents _characterComponents;
        private PointNClickConfigSO _configSO;
        
        public IStateMachine Owner { get; set; }

        [Inject]
        public CharacterFreeState(PointNClickActions actions, CharacterComponents characterComponents, 
            PointNClickConfigSO configSO)
        {
            _actions = actions;
            _characterComponents = characterComponents;
            _configSO = configSO;
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
            
            RaycastHit hit;
            if (PhysicsService.RayCastFromCamera(screenPoint, _configSO.InteractableMask, out hit)
                && hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                _characterComponents.TargetInteractable = interactable;
                Owner.Switch(typeof(CharacterInteractingState));
                return;
            }
            
            if (PhysicsService.RayCastFromCamera(screenPoint, _configSO.WalkableMask, out hit))
                Move(hit.point);
        }

        private void Cancel(InputAction.CallbackContext ctx) => _characterComponents.Mover.Stop();

        private void Move(Vector3 target) => _characterComponents.Mover.Move(target);

        
    }
}