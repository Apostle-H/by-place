using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.ActionSystem;
using DialogueSystem.Data.Save;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace DialogueSystem
{
    public class DialogueResolver : MonoBehaviour
    {
        [SerializeField] private UIDocument panel;
        [SerializeField] private VisualTreeAsset choiceBtnAsset;

        private DRContainerSO _currentDRContainer;
        
        private ActionResolver _actionResolver;

        private int _afterActionGuid;
        
        private Image _speakerIcon;
        private Label _speakerName;
        private Label _speakerText;
        private VisualElement _choiceBtnsContainer;
        private List<Button> _choiceBtns = new();
        
        private List<Action> _choiceBtnsActions = new();

        public event Action OnQuit;

        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        private void Awake()
        {
            Hide();
            
            _speakerIcon = new Image();
            _speakerName = panel.rootVisualElement.Q<Label>("SpeakerName");
            _speakerText = panel.rootVisualElement.Q<Label>("SpeakerText");
            _choiceBtnsContainer = panel.rootVisualElement.Q("ChoiceBtnsContainer");
            
            panel.rootVisualElement.Q<VisualElement>("IconContainer").Add(_speakerIcon);
        }

        public void Load(DRGroupSO dsGroup)
        {
            Bind();
            
            _currentDRContainer = dsGroup.Owner;
            Choose(dsGroup.StartingNodeGuid);
            panel.rootVisualElement.visible = true;
        }

        private void Hide()
        {
            Expose();
            
            panel.rootVisualElement.visible = false;
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
                _speakerText.text = dialogueNodeSO.Text;

                for (var i = 0; i < dialogueNodeSO.NextGuids.Count; i++)
                {
                    if (_choiceBtns.Count <= i)
                        AddChoiceBtn();

                    _choiceBtns[i].text = dialogueNodeSO.ChoicesText[i];

                    _choiceBtns[i].clicked -= _choiceBtnsActions[i];
                    var choiceNum = i;
                    _choiceBtnsActions[i] = () => Choose(dialogueNodeSO.NextGuids[choiceNum].NextGuid);
                    _choiceBtns[i].clicked += _choiceBtnsActions[i];

                    _choiceBtns[i].visible = true;
                }

                for (var i = dialogueNodeSO.NextGuids.Count; i < _choiceBtns.Count; i++)
                    _choiceBtns[i].visible = false;
            }
            else if (targetNodeSO is DRActionNodeSO actionNodeSO)
            {
                _afterActionGuid = actionNodeSO.NextGuids[0].NextGuid;
                _actionResolver.Resolve(actionNodeSO.TargetSO.Id);
            }
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