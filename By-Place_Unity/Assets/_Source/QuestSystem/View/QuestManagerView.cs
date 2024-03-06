using System;
using System.Collections.Generic;
using Core.Loaders;
using QuestSystem.View.Data;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

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
            var freeQuestView = new QuestView(_configSO.QuestVisualTree.Instantiate().Q<VisualElement>("Quest"));
            Root.Add(freeQuestView.Root);
            freeQuestView.Hide();
                
            _freeQuestViews.Add(freeQuestView);
        }
    }
}