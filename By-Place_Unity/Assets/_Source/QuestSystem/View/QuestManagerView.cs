using System;
using System.Collections.Generic;
using System.Linq;
using QuestSystem.Data;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace QuestSystem.View
{
    public class QuestManagerView : MonoBehaviour
    {
        [SerializeField] private UIDocument questUI;
        [SerializeField] private VisualTreeAsset questVisualTree;

        private QuestManager _questManager;

        private List<QuestView> _questViews = new();

        public VisualElement Root { get; private set; }

        [Inject]
        private void Inject(QuestManager questManager) => _questManager = questManager;

        private void Awake()
        {
            Root = questUI.rootVisualElement.Q<VisualElement>("QuestPanel");
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind()
        {
            _questManager.OnAdd += Add;
            _questManager.OnRemove += Remove;
        }
        
        private void Expose()
        {
            _questManager.OnAdd -= Add;
            _questManager.OnRemove -= Remove;
        }

        private void Add(Quest quest)
        {
            if (_questViews.All(questView => questView.TargetQuest != default))
            {
                var newQuestView = new QuestView(questVisualTree, _questViews.Count);
                Root.Add(newQuestView.Root);
            
                _questViews.Add(newQuestView);
            }

            var questView = _questViews.First(questView => questView.TargetQuest == default);
            questView.SetTargetQuest(quest);
            questView.Show();
        }
        
        private void Remove(Quest quest)
        {
            var questView= _questViews.First(view => view.TargetQuest == quest);
            
            questView.SetTargetQuest(default);
            questView.Hide();
        }
    }
}