using System;

namespace DialogueSystem.ActionSystem
{
    public interface IAction
    {
        public int Id { get; }
        public bool Resolve { get; }

        public event Action<IAction> OnFinished;
        
        public void Perform();
    }
}