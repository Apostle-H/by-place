using System;
using System.Collections.Generic;
using QuestSystem.Data;
using QuestSystem.View.Data;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace QuestSystem.View
{
    public class QuestManagerView : IStartable, IDisposable
    {
        private QuestManagerViewConfigSO _configSO;
        
        private UIDocument _canvas;

        private QuestManager _questManager;

        private Dictionary<int, QuestView> _questViews = new();
        private List<QuestView> _freeQuestViews = new();
        
        public VisualElement Root { get; private set; }

        [Inject]
        public QuestManagerView(QuestManagerViewConfigSO configSO, UIDocument canvas, QuestManager questManager)
        {
            _configSO = configSO;
            _canvas = canvas;
            _questManager = questManager;
        }

        public void Start()
        {
            Root = _canvas.rootVisualElement.Q<VisualElement>("QuestPanel");
            
            Bind();
        }

        public void Dispose() => Expose();

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

        private void Open(Quest quest)
        {
            if (_freeQuestViews.Count < 1)
                Add();
            
            var questView = _freeQuestViews[^1];
            _freeQuestViews.RemoveAt(_freeQuestViews.Count - 1);
            
            _questViews.Add(quest.Id, questView);
            questView.Update(quest.Title, quest.Task);
            
            questView.Show();
        }
        
        private void UpdateQuest(Quest quest) => _questViews[quest.Id].Update(quest.Title, quest.Task);

        private void Close(Quest quest)
        {
            var questView = _questViews[quest.Id];
            _questViews.Remove(quest.Id);
            
            _freeQuestViews.Add(questView);
            questView.Hide();
        }

        private void Add()
        {
            var freeQuestView = new QuestView(_configSO.QuestVisualTree.Instantiate().Q<VisualElement>("Quest"));
            Root.Add(freeQuestView.Root);
            freeQuestView.Hide();
                
            _freeQuestViews.Add(freeQuestView);
        }
    }
}