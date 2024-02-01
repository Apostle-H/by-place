using System.Collections.Generic;
using System.Linq;
using DialogueSystem.ActionSystem;
using QuestSystem.Actions;
using UnityEngine;
using VContainer;

namespace QuestSystem
{
    public class QuestActionsCollector : MonoBehaviour
    {
        private List<QuestAction> _questActions = new();

        private QuestManager _questManager;
        private ActionResolver _actionResolver;
        

        [Inject]
        private void Inject(QuestManager questManager, ActionResolver actionResolver)
        {
            _questManager = questManager;
            _actionResolver = actionResolver;
        }

        private void Awake()
        {
            var questActionsSO = Resources.LoadAll<QuestActionSO>("QuestSystem/Actions").ToList();

            foreach (var questActionSO in questActionsSO)
                _questActions.Add(new QuestAction(questActionSO.Id, questActionSO.QuestId, questActionSO.Title,
                    questActionSO.Task, _questManager));
        }

        public void Start()
        {
            foreach (var questActionSO in _questActions)
                _actionResolver.AddAction(questActionSO);
        }

        public void OnDestroy()
        {
            foreach (var questActionSO in _questActions)
                _actionResolver.RemoveAction(questActionSO);
        }
    }
}