using System;

namespace StateMachine
{
    public interface IStateMachine
    {
        public void Switch(Type stateType);
    }
}