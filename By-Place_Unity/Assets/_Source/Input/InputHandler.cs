using System;
using InputSystem;
using VContainer;
using VContainer.Unity;

namespace Input
{
    public class InputHandler : IStartable, IDisposable
    {
        private readonly PointNClickActions _actions;

        [Inject]
        public InputHandler(PointNClickActions actions) => _actions = actions;

        public void Start() => _actions.Enable();

        public void Dispose() => _actions.Disable();
    }
}