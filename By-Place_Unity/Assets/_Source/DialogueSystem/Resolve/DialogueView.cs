﻿using System;
using System.Collections.Generic;
using DialogueSystem.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private UIDocument canvas;
        [SerializeField] private VisualTreeAsset choiceBtnAsset;
        
        private Image _speakerIcon;
        private Label _speakerName;
        private Label _speakerText;
        private VisualElement _choiceBtnsContainer;
        private List<Button> _choiceBtns = new();

        private List<Action> _clickedActions = new();

        public VisualElement Root { get; private set; }
        

        public event Action<int> OnChoose;

        private void Awake()
        {
            Root = canvas.rootVisualElement.Q<VisualElement>("DialoguePanel");
            
            Hide();
            
            _speakerIcon = new Image();
            _speakerName = Root.Q<Label>("SpeakerName");
            _speakerText = Root.Q<Label>("SpeakerText");
            _choiceBtnsContainer = Root.Q("ChoiceBtnsContainer");
            
            Root.Q<VisualElement>("IconContainer").Add(_speakerIcon);
        }

        public void Show()
        {
            Root.visible = true;
            Bind();
        }

        public void Hide()
        {
            foreach (var choiceBtn in _choiceBtns)
                choiceBtn.visible = false;

            Root.visible = false;
            Expose();
        }

        private void Bind()
        {
            for (var i = 0; i < _choiceBtns.Count; i++)
                _choiceBtns[i].clicked += _clickedActions[i].Invoke;
        }

        private void Expose()
        {
            for (var i = 0; i < _choiceBtns.Count; i++)
                _choiceBtns[i].clicked -= _clickedActions[i].Invoke;
        }

        public void DisplayDialogue(DDialogue dialogue)
        {
            _speakerName.text = dialogue.SpeakerName;
            _speakerIcon.sprite = dialogue.SpeakerIcon;
            _speakerText.text = dialogue.SpeakerText;
            
            for (var i = 0; i < dialogue.Choices.Count; i++)
            {
                if (_choiceBtns.Count <= i)
                    AddChoiceBtn();
                
                _choiceBtns[i].text = dialogue.Choices[i];
                _choiceBtns[i].visible = true;
            }
        }

        private void AddChoiceBtn()
        {
            var btnIndex = _choiceBtns.Count;
            
            var choiceBtn = choiceBtnAsset.CloneTree().Q<Button>("DialogueChoiceBtn");
            _choiceBtnsContainer.Add(choiceBtn);
            _choiceBtns.Add(choiceBtn);
            _clickedActions.Add(() => OnChoose?.Invoke(btnIndex));

            _choiceBtns[btnIndex].clicked += _clickedActions[btnIndex];
        }
    }
}