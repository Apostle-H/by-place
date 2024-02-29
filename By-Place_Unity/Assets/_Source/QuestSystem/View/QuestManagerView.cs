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

        private Dictionary<int, QuestView> _questViews = new();
        private List<QuestView> _freeQuestViews = new();

        public VisualElement Root { get; private set; }

        [Inject]
        private void Inject(QuestManager questManager) => _questManager = questManager;

        private void Awake() => Root = questUI.rootVisualElement.Q<VisualElement>("QuestPanel");

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind()
        {
            _questManager.OnOpen += Open;
            _questManager.OnUpdate += UpdateQuest;
            _questManager.OnClose += Close;
        }

        private void Expose()
        {
            _questManager.OnOpen -= Open;
            _questManager.OnUpdate -= UpdateQuest;
            _questManager.OnClose -= Close;
        }

        private void Open(int questId)
        {
            if (_freeQuestViews.Count < 1)
            {
                Add();
            }
            
            var questView = _freeQuestViews[^1];
            _freeQuestViews.RemoveAt(_freeQuestViews.Count - 1);
            
            _questViews.Add(questId, questView);
            questView.Show();
        }
        
        private void UpdateQuest(int questId, string title, string task) => _questViews[questId].Update(title, task);

        private void Close(int questId)
        {
            var questView = _questViews[questId];
            _questViews.Remove(questId);
            
            _freeQuestViews.Add(questView);
            questView.Hide();
        }

        private void Add()
        {
            var freeQuestView = new QuestView(questVisualTree);
            Root.Add(freeQuestView.Root);
            freeQuestView.Hide();
                
            _freeQuestViews.Add(freeQuestView);
        }
    }
}