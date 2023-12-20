using System.Collections;
using InputSystem;
using PointNClick.Data;
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
            _actions.Main.Stop.performed += Stop;
        }

        private void Expose()
        {
            _actions.Main.Interact.canceled -= Clicked;
            _actions.Main.Stop.performed -= Stop;
        }

        private void Clicked(InputAction.CallbackContext ctx)
        {
            var screenPoint = _actions.Main.Point.ReadValue<Vector2>();
            if (!PhysicsService.RayCastFromCamera(screenPoint, _configSO.WalkableMask, out RaycastHit hit))
                return;
            
            _coroutineRunner.StartCoroutine(ToggleMoveCursor());
            _characterComponents.Mover.Move(hit.point);
        }

        private void Stop(InputAction.CallbackContext ctx) => _characterComponents.Mover.Stop();

        private IEnumerator ToggleMoveCursor()
        {
            Cursor.SetCursor(_configSO.MoveCursor, Vector2.zero, CursorMode.ForceSoftware);
            yield return _cursorToggleWaiter;
            Cursor.SetCursor(_configSO.IdleCursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        ~PointNClickInvoker() => Expose();
    }
}