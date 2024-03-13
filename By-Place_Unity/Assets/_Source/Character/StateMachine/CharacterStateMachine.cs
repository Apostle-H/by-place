using System;
using System.Collections.Generic;
using StateMachine;
using VContainer;
using VContainer.Unity;

namespace Character
{
    public class CharacterStateMachine : IStateMachine, ITickable, IInitializable
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        [Inject]
        public CharacterStateMachine(IStatesProvider statesProvider)
        {
            _states = statesProvider.Get();
            _currentState = statesProvider.StartingState;

            foreach (var kvp in _states)
                kvp.Value.Owner = this;
        }

        public void Initialize() => _currentState.Enter();

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