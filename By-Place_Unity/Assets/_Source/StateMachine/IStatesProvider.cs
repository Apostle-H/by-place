using System;
using System.Collections.Generic;

namespace StateMachine
{
    public interface IStatesProvider
    {
        public IState StartingState { get; }
        
        public Dictionary<Type, IState> Get();
    }
}