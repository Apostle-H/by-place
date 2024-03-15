using SaveLoad.Invoker;
using StateMachine;

namespace Core.StateMachine.States
{
    public class CoreLoadState : IState
    {
        private SaveLoadInvoker _saveLoadInvoker;

        public IStateMachine Owner { get; set; }

        public CoreLoadState(SaveLoadInvoker saveLoadInvoker) => _saveLoadInvoker = saveLoadInvoker;

        public void Enter()
        {
            _saveLoadInvoker.InvokerLoad();
            Owner.Switch<WaitingState>();
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}