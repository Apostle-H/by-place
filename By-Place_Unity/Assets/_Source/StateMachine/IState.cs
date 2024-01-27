namespace StateMachine
{
    public interface IState
    {
        public IStateMachine Owner { get; set; }
        
        public void Enter();

        public void Update();
        
        public void Exit();
    }
}