using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.ActionSystem;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace DialogueSystem
{
    public class DialogueResolver : MonoBehaviour
    {
        [SerializeField] private UIDocument canvas;
        [SerializeField] private VisualTreeAsset choiceBtnAsset;

        private DRContainerSO _currentDRContainer;
        
        private ActionResolver _actionResolver;
        private DVariablesContainer _variablesContainer;

        private int _afterActionGuid;
        
        private Image _speakerIcon;
        private Label _speakerName;
        private Label _speakerText;
        private VisualElement _choiceBtnsContainer;
        private List<Button> _choiceBtns = new();
        
        private List<Action> _choiceBtnsActions = new();

        public VisualElement Root { get; private set; }

        public event Action OnQuit;

        [Inject]
        private void Inject(ActionResolver actionResolver, DVariablesContainer variablesContainer)
        {
            _actionResolver = actionResolver;
            _variablesContainer = variablesContainer;
        }

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

        public void Load(DRGroupSO dsGroup)
        {
            Bind();
            
            _currentDRContainer = dsGroup.Owner;
            Choose(dsGroup.StartingNodeGuid);
            Root.visible = true;
        }

        private void Hide()
        {
            Expose();
            
            Root.visible = false;
        }

        private void Bind() => _actionResolver.OnFinished += NextAfterAction;

        private void Expose()
        {
            _actionResolver.OnFinished -= NextAfterAction;
            
            for (var i = 0; i < _choiceBtns.Count; i++)
            {
                _choiceBtns[i].clicked -= _choiceBtnsActions[i];
                _choiceBtns[i].visible = false;
            }
        }

        private void Next(int guid)
        {
            var targetNodeSO = _currentDRContainer.Nodes.First(node => node.Guid == guid);
            if (targetNodeSO is DRDialogueNodeSO dialogueNodeSO)
            {
                _speakerIcon.sprite = dialogueNodeSO.SpeakerSO.Icon;
                _speakerName.text = dialogueNodeSO.SpeakerSO.Name;
                _speakerText.text = GetText(dialogueNodeSO);

                var displayedChoices = 0;
                for (var i = 0; i < dialogueNodeSO.NextGuids.Count; i++)
                {
                    if (!CheckVariable(dialogueNodeSO.Choices[i].CheckVariableSO, dialogueNodeSO.Choices[i].ExpectedValue))
                        continue;
                    
                    if (_choiceBtns.Count <= displayedChoices)
                        AddChoiceBtn();

                    _choiceBtns[displayedChoices].text = dialogueNodeSO.Choices[i].Text;

                    _choiceBtns[displayedChoices].clicked -= _choiceBtnsActions[displayedChoices];
                    var choiceNum = i;
                    _choiceBtnsActions[displayedChoices] = () => Choose(dialogueNodeSO.NextGuids[choiceNum].NextGuid);
                    _choiceBtns[displayedChoices].clicked += _choiceBtnsActions[displayedChoices];

                    _choiceBtns[displayedChoices].visible = true;
                    displayedChoices++;
                }

                for (var i = displayedChoices; i < _choiceBtns.Count; i++)
                    _choiceBtns[i].visible = false;
            }
            else if (targetNodeSO is DRActionNodeSO actionNodeSO)
            {
                _afterActionGuid = actionNodeSO.NextGuids[0].NextGuid;
                _actionResolver.Resolve(actionNodeSO.ActionSO.Id);
            }
            else if (targetNodeSO is DRSetVariableNodeSO setVariableNodeSO)
            {
                if (setVariableNodeSO.VariableSO == default)
                    return;
            
                _variablesContainer.Set(setVariableNodeSO.VariableSO.Id, setVariableNodeSO.SetValue);
                Choose(setVariableNodeSO.NextGuids[0].NextGuid);
            }
        }

        private string GetText(DRDialogueNodeSO dialogueNodeSO)
        {
            var text = dialogueNodeSO.Texts[0].Text;
            for (var i = 1; i < dialogueNodeSO.Texts.Count; i++)
            {
                if (!_variablesContainer.Get(dialogueNodeSO.Texts[i].VariableSO.Id, out var variable) || !variable)
                    continue;
                
                text = dialogueNodeSO.Texts[i].Text;
            }

            return text;
        }

        private bool CheckVariable(DVariableSO variableSO, bool expectedValue)
        {
            if (variableSO == default)
                return true;

            if (!_variablesContainer.Get(variableSO.Id, out var variable))
                return false;
            
            return variable == expectedValue;
        }

        private void Choose(int guid)
        {
            if (guid != -1)
            {
                Next(guid);
                return;
            }

            Hide();
            OnQuit?.Invoke();
        }
        
        private void NextAfterAction() => Choose(_afterActionGuid);

        private void AddChoiceBtn()
        {
            var choiceBtn = choiceBtnAsset.CloneTree().Q<Button>("DialogueChoiceBtn");
            _choiceBtnsContainer.Add(choiceBtn);
            _choiceBtns.Add(choiceBtn);
            _choiceBtnsActions.Add(default);
        }
    }
}