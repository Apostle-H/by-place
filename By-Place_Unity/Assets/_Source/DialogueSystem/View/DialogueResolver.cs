using System;
using System.Collections.Generic;
using DialogueSystem.Data.Save;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.View
{
    public class DialogueResolver : MonoBehaviour
    {
        [SerializeField] private UIDocument panel;
        [SerializeField] private VisualTreeAsset choiceBtnAsset;

        private Image _speakerIcon;
        private Label _speakerName;
        private Label _speakerText;
        private VisualElement _choiceBtnsContainer;
        private List<Button> _choiceBtns = new();
        
        private List<Action> _choiceBtnsActions = new();

        public event Action OnQuit;

        private void Awake()
        {
            Hide();
            
            _speakerIcon = new Image();
            _speakerName = panel.rootVisualElement.Q<Label>("SpeakerName");
            _speakerText = panel.rootVisualElement.Q<Label>("SpeakerText");
            _choiceBtnsContainer = panel.rootVisualElement.Q("ChoiceBtnsContainer");
            
            panel.rootVisualElement.Q<VisualElement>("IconContainer").Add(_speakerIcon);
        }

        public void Load(DialogueGroupSO dialogueGroup)
        {
            Next(dialogueGroup.StartingNode);
            panel.rootVisualElement.visible = true;
        }

        private void Hide()
        {
            panel.rootVisualElement.visible = false;
            for (var i = 0; i < _choiceBtns.Count; i++)
            {
                _choiceBtns[i].clicked -= _choiceBtnsActions[i];
                _choiceBtns[i].visible = false;
            }
        }

        private void Next(DialogueNodeSO targetNodeSO)
        {
            _speakerIcon.sprite = targetNodeSO.SpeakerSO.Icon;
            _speakerName.text = targetNodeSO.SpeakerSO.Name;
            _speakerText.text = targetNodeSO.Text;

            for (var i = 0; i < targetNodeSO.Choices.Count; i++)
            {
                if (_choiceBtns.Count <= i)
                    AddChoiceBtn();

                _choiceBtns[i].text = targetNodeSO.Choices[i].Text;

                _choiceBtns[i].clicked -= _choiceBtnsActions[i];
                var choiceNum = i;
                _choiceBtnsActions[i] = () => Choose(targetNodeSO.Choices[choiceNum].NextNode);
                _choiceBtns[i].clicked += _choiceBtnsActions[i];

                _choiceBtns[i].visible = true;
            }

            for (var i = targetNodeSO.Choices.Count; i < _choiceBtns.Count; i++)
            {
                _choiceBtns[i].visible = false;
            }
        }

        private void Choose(DialogueNodeSO nextNodeSO)
        {
            if (nextNodeSO != default)
            {
                Next(nextNodeSO);
                return;
            }

            Hide();
            OnQuit?.Invoke();
        }

        private void AddChoiceBtn()
        {
            var choiceBtn = choiceBtnAsset.CloneTree().Q<Button>("DialogueChoiceBtn");
            _choiceBtnsContainer.Add(choiceBtn);
            _choiceBtns.Add(choiceBtn);
            _choiceBtnsActions.Add(default);
        }
    }
}