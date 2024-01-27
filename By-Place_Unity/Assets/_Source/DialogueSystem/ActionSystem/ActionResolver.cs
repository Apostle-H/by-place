using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.ActionSystem
{
    public class ActionResolver
    {
        private Dictionary<int, IAction> _actions = new();

        public event Action OnFinished;

        public void AddAction(IAction action)
        {
            if (_actions.ContainsKey(action.Id))
                return;
            
            _actions.Add(action.Id, action);
        }

        public void RemoveAction(IAction action) => _actions.Remove(action.Id);

        public void Resolve(int id)
        {
            if (!_actions.ContainsKey(id) || !_actions[id].Resolve)
            {
                OnFinished?.Invoke();
                return;
            }

            _actions[id].OnFinished += Finished;
            _actions[id].Perform();
        }

        private void Finished(IAction action)
        {
            action.OnFinished -= Finished;
            OnFinished?.Invoke();
        }
    }
}