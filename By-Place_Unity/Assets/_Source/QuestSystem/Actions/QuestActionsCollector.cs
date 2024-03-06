using System;
using System.Collections.Generic;
using System.Linq;
using ActionSystem;
using QuestSystem.Actions.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QuestSystem.Actions
{
    public class QuestActionsCollector : IInitializable, IStartable, IDisposable
    {
        private List<IAction> _questActions = new();

        private IObjectResolver _container;
        
        private ActionResolver _actionResolver;

        [Inject]
        private void Inject(IObjectResolver container, ActionResolver actionResolver)
        {
            _container = container;
            _actionResolver = actionResolver;
        }
        
        public void Initialize()
        {
            var questActionsSO = Resources.LoadAll<QuestActionSO>("Actions/Quest").ToList();

            foreach (var questActionSO in questActionsSO)
            {
                var action = questActionSO.Build();
                _container.Inject(action);
                
                _questActions.Add(action);
            }
        }

        public void Start()
        {
            foreach (var questActionSO in _questActions)
                _actionResolver.AddAction(questActionSO);
        }

        public void Dispose()
        {
            foreach (var questActionSO in _questActions)
                _actionResolver.RemoveAction(questActionSO);
        }
    }
}