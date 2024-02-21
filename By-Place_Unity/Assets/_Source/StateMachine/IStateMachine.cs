using System;

namespace StateMachine
{
    public interface IStateMachine
    {
        public void Switch<T>() where T : IState;
    }
}