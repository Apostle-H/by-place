using System;
using System.Collections.Generic;
using StateMachine;
using VContainer;
using VContainer.Unity;

namespace Core.StateMachine
{
    public class CoreStateMachine : IStateMachine, IPostStartable, ITickable
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        [Inject]
        public CoreStateMachine(ICoreStatesProvider statesProvider)
        {
            _states = statesProvider.Get();
            _currentState = statesProvider.StartingState;

            foreach (var kvp in _states)
                kvp.Value.Owner = this;
        }

        public void PostStart() => _currentState.Enter();

        public void Switch<T>() where T : IState
        {
            var stateType = typeof(T);
            if (!_states.ContainsKey(stateType))
                return;

            _currentState.Exit();
            _currentState = _states[stateType];
            _currentState.Enter();
        }

        public void Tick() => _currentState?.Update();
    }
}