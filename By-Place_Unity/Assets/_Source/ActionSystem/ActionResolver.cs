using System;
using System.Collections.Generic;
using Registration;
using SaveLoad;

namespace ActionSystem
{
    public class ActionResolver : IRegistrator<IAction>
    {
        private Dictionary<int, IAction> _actions = new();

        public string Path => "Actions";

        public event Action OnFinished;

        public void Register(IAction action)
        {
            if (_actions.ContainsKey(action.Id))
                return;
            
            _actions.Add(action.Id, action);
        }

        public void Unregister(IAction action) => _actions.Remove(action.Id);

        public void Resolve(int id)
        {
            if (!_actions.ContainsKey(id) || !_actions[id].Resolvable)
            {
                OnFinished?.Invoke();
                return;
            }

            _actions[id].OnFinished += Finished;
            _actions[id].Resolve();
        }

        private void Finished(IAction action)
        {
            action.OnFinished -= Finished;
            OnFinished?.Invoke();
        }
    }
}