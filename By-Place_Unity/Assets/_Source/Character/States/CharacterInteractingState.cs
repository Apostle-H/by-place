using InputSystem;
using PointNClick.Data;
using StateMachine;
using UnityEngine.InputSystem;
using VContainer;

namespace Character.States
{
    public class CharacterInteractingState : IState
    {
        private PointNClickActions _actions;
        private CharacterComponents _characterComponents;
        
        public IStateMachine Owner { get; set; }

        [Inject]
        public CharacterInteractingState(PointNClickActions actions, CharacterComponents characterComponents)
        {
            _actions = actions;
            _characterComponents = characterComponents;
        }

        public void Enter()
        {
            Bind();
            
            _characterComponents.Interacter.Interact(_characterComponents.TargetInteractable);
        }

        public void Update() { }

        public void Exit() => Expose();

        private void Bind()
        {
            _actions.Main.Stop.performed += Cancel;
            _characterComponents.TargetInteractable.OnFinished += ToFreeState;
        }

        private void Expose()
        {
            _actions.Main.Stop.performed -= Cancel;
            _characterComponents.TargetInteractable.OnFinished -= ToFreeState;
        }

        private void Cancel(InputAction.CallbackContext ctx)
        {
            _characterComponents.Interacter.Cancel();
            ToFreeState();
        }
        
        private void ToFreeState() => Owner.Switch<CharacterFreeState>();
    }
}