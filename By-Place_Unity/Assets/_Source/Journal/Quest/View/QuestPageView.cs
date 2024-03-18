using System;
using System.Collections.Generic;
using Journal.Quest.View.Data;
using QuestSystem;
using Sound;
using Sound.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Journal.Quest.View
{
    public class QuestPageView : IStartable, IDisposable
    {
        private QuestPageViewConfigSO _configSO;
        private UIDocument _canvas;

        private QuestManager _questManager;
        private readonly VisualElementsAudio _visualElementsAudio;

        private VisualElement _root;
        private ScrollView _questsContainer;

        private Dictionary<int, QuestFoldoutView> _questFoldouts = new();

        [Inject]
        public QuestPageView(QuestPageViewConfigSO configSO, UIDocument canvas, QuestManager questManager, 
            VisualElementsAudio visualElementsAudio)
        {
            _configSO = configSO;
            _canvas = canvas;
            _questManager = questManager;
            _visualElementsAudio = visualElementsAudio;
        }

        public void Start()
        {
            _root = _canvas.rootVisualElement.Q<VisualElement>("QuestPagePanel");
            _questsContainer = _root.Q<ScrollView>("QuestsContainer");
            
            Bind();
        }

        public void Dispose()
        {
            Expose();

            foreach (var kvp in _questFoldouts)
                _visualElementsAudio.Unregister(kvp.Value.Foldout.Q<Label>());
        }

        private void Bind()
        {
            _questManager.OnOpen += OpenQuest;
            _questManager.OnUpdate += LogQuest;
            _questManager.OnClose += CloseQuest;
        }
        
        private void Expose()
        {
            _questManager.OnOpen -= OpenQuest;
            _questManager.OnUpdate -= LogQuest;
            _questManager.OnClose -= CloseQuest;
        }

        private void OpenQuest(QuestSystem.Data.Quest quest)
        {
            var questFoldout = _configSO.QuestDropDown.Instantiate().Q<Foldout>("QuestFoldout");
            var questFoldoutView = new QuestFoldoutView(questFoldout, quest.Title);
            
            _questsContainer.Add(questFoldoutView.Root);
            _questFoldouts.Add(quest.Id, questFoldoutView);
            
            _visualElementsAudio.Register(questFoldout.Q<Label>());
        }
        
        private void LogQuest(QuestSystem.Data.Quest quest)
        {
            Debug.Log(quest.Conclusions);
            
            _questFoldouts[quest.Id].Log(quest.Conclusions[^1]);
        }

        private void CloseQuest(QuestSystem.Data.Quest quest) => _questFoldouts[quest.Id].Close(quest.Result);
    }
}