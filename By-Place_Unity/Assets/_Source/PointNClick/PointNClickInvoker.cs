using System.Collections;
using InputSystem;
using PointNClick.Data;
using PointNClick.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Runners;
using Utils.Services;
using VContainer;
using VContainer.Unity;

namespace PointNClick
{
    public class PointNClickInvoker : IStartable
    {
        private readonly PointNClickActions _actions;
        private readonly CharacterComponents _characterComponents;
        private readonly PointNClickConfigSO _configSO;
        private readonly CoroutineRunner _coroutineRunner;

        private readonly WaitForSeconds _cursorToggleWaiter;

        private IInteractable _targetInteractable;

        private bool _canMove;

        [Inject]
        public PointNClickInvoker(PointNClickActions actions, CharacterComponents characterComponents, 
            PointNClickConfigSO configSO, CoroutineRunner coroutineRunner)
        {
            _actions = actions;
            _characterComponents = characterComponents;
            _configSO = configSO;
            _coroutineRunner = coroutineRunner;

            _cursorToggleWaiter = new WaitForSeconds(_configSO.CursorToggleTime);
            
             Bind();
        }

        public void Start()
        {
            _actions.Main.Enable();
            Cursor.SetCursor(_configSO.IdleCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void Bind()
        {
            _actions.Main.Interact.canceled += Clicked;
            _actions.Main.Stop.performed += Cancel;
        }

        private void Expose()
        {
            _actions.Main.Interact.canceled -= Clicked;
            _actions.Main.Stop.performed -= Cancel;
        }

        private void Clicked(InputAction.CallbackContext ctx)
        {
            var screenPoint = _actions.Main.Point.ReadValue<Vector2>();
            var hit = new RaycastHit();
            
            if (PhysicsService.RayCastFromCamera(screenPoint, _configSO.InteractableMask, out hit)
                && hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                Interact(interactable);
                return;
            }
            
            if (_canMove && PhysicsService.RayCastFromCamera(screenPoint, _configSO.WalkableMask, out hit))
                Move(hit.point);
        }

        private void Cancel(InputAction.CallbackContext ctx)
        {
            if (_targetInteractable != default)
            {
                _targetInteractable.OnFinished -= UnblockMovement;
                _characterComponents.Mover.OnArrived += StartInteraction;
                
                UnblockMovement();
            }
            _characterComponents.Mover.Stop();
        }

        private void Move(Vector3 target)
        {
            _coroutineRunner.StartCoroutine(ToggleMoveCursor());
            _characterComponents.Mover.Move(target);
        }

        private void Interact(IInteractable interactable)
        {
            _targetInteractable = interactable;
            if (_targetInteractable.BlockMovement)
            {
                BlockMovement();
                _targetInteractable.OnFinished += UnblockMovement;
            }
            _characterComponents.Mover.OnArrived += StartInteraction;
                
            _coroutineRunner.StartCoroutine(ToggleMoveCursor());
            _characterComponents.Mover.Move(_targetInteractable.Position);
            _canMove = !_targetInteractable.BlockMovement;
        }

        private void StartInteraction()
        {
            _characterComponents.Mover.OnArrived -= StartInteraction;
            _targetInteractable.Interact();
        }

        private void BlockMovement() => _canMove = false;

        private void UnblockMovement()
        {
            _canMove = true;
            _targetInteractable.OnFinished -= UnblockMovement;
        }

        private IEnumerator ToggleMoveCursor()
        {
            Cursor.SetCursor(_configSO.MoveCursor, Vector2.zero, CursorMode.ForceSoftware);
            yield return _cursorToggleWaiter;
            Cursor.SetCursor(_configSO.IdleCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        ~PointNClickInvoker() => Expose();
    }
}