using System;
using System.Collections.Generic;
using Journal.Quest.View.Data;
using Sound;
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

        private JournalQuests _quests;
        private readonly VisualElementsAudio _visualElementsAudio;

        private VisualElement _root;
        private ScrollView _questsContainer;

        private Dictionary<int, QuestFoldoutView> _questFoldouts = new();

        [Inject]
        public QuestPageView(QuestPageViewConfigSO configSO, UIDocument canvas, JournalQuests quests, 
            VisualElementsAudio visualElementsAudio)
        {
            _configSO = configSO;
            _canvas = canvas;
            _quests = quests;
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
            _quests.OnQuestOpened += OpenQuest;
            _quests.OnLogAdded += LogQuest;
            _quests.OnQuestClosed += CloseQuest;
        }
        
        private void Expose()
        {
            _quests.OnQuestOpened -= OpenQuest;
            _quests.OnLogAdded -= LogQuest;
            _quests.OnQuestClosed -= CloseQuest;
        }

        private void OpenQuest(int questId, string title)
        {
            var questFoldout = _configSO.QuestDropDown.Instantiate().Q<Foldout>("QuestFoldout");
            var questFoldoutView = new QuestFoldoutView(questFoldout, title);
            
            _questsContainer.Add(questFoldoutView.Root);
            _questFoldouts.Add(questId, questFoldoutView);
            
            _visualElementsAudio.Register(questFoldout.Q<Label>());
        }
        
        private void LogQuest(int questId, string log) => _questFoldouts[questId].Log(log);

        private void CloseQuest(int questId, string result)
        {
            _questFoldouts[questId].Close(result);
        }
    }
}