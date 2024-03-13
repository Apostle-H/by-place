using System;
using System.Collections.Generic;
using Core.StateMachine.States;
using StateMachine;
using VContainer;

namespace Core.StateMachine
{
    public class CoreStatesProvider : ICoreStatesProvider
    {
        private readonly Dictionary<Type, IState> _states = new();
        
        public IState StartingState { get; private set; }

        [Inject]
        public CoreStatesProvider(WaitingState waitingState, CoreLoadState coreLoadState)
        {
            StartingState = coreLoadState;
            
            _states.Add(coreLoadState.GetType(), coreLoadState);
            _states.Add(waitingState.GetType(), waitingState);
        }
        
        public Dictionary<Type, IState> Get() => _states;
    }
}