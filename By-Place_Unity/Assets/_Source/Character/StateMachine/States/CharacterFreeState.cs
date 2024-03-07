using Character.Data;
using InputSystem;
using Interactions;
using PointNClick.Data;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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

        private UIService _uiService;

        public IStateMachine Owner { get; set; }

        [Inject]
        public CharacterFreeState(PointNClickConfigSO configSO, PointNClickActions actions,
            CharacterComponents characterComponents, UIService uiService)
        {
            _configSO = configSO;
            _actions = actions;
            _characterComponents = characterComponents;

            _uiService = uiService;
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
            
            if (_uiService.IsOverUIElement(screenPoint) 
                || !PhysicsService.RayCastFromCamera(screenPoint, out var hit) 
                || hit.collider == default)
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