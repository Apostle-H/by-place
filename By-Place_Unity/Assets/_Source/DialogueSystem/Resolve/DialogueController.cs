using System;
using System.Collections.Generic;
using ActionSystem;
using DialogueSystem.Data;
using DialogueSystem.Data.NodeParams;
using DialogueSystem.Data.Save;
using VContainer;

namespace DialogueSystem
{
    public class DialogueController
    {
        private DialogueData _dialogueData;
        private DialogueView _dialogueView;
        
        private ActionResolver _actionResolver;
        private DVariablesContainer _variablesContainer;

        private int _afterActionGuid;
        
        private DDialogue _dialogue;
        private List<int> _nextGuids = new List<int>();
        
        public event Action OnQuit;

        [Inject]
        private void Inject(DialogueData dialogueData, DialogueView dialogueView, ActionResolver actionResolver,
            DVariablesContainer variablesContainer)
        {
            _dialogueData = dialogueData;
            _dialogueView = dialogueView;
            
            _actionResolver = actionResolver;
            _variablesContainer = variablesContainer;
        }

        public void Load(DRGroupSO dsGroup)
        {
            Bind();
            
            _dialogueData.Load(dsGroup.Owner);
            _dialogueView.Show();
            
            Resolve(dsGroup.StartingNodeGuid);
        }
        
        private void Bind()
        {
            _dialogueView.OnChoose += Choose;
            _actionResolver.OnFinished += NextAfterAction;
        }

        private void Expose()
        {
            _dialogueView.OnChoose -= Choose;
            _actionResolver.OnFinished -= NextAfterAction;
        }

        private void Resolve(int guid)
        {
            var actionId = 0;
            var variableId = 0;
            var variableSet = false;
            
            while (guid != -1)
            {
                switch (_dialogueData.Choose(guid, ref _dialogue, ref _nextGuids, ref actionId, 
                            ref variableId, ref variableSet))
                {
                    case DNodeType.DIALOGUE:
                        _dialogueView.DisplayDialogue(_dialogue);
                        return;
                    case DNodeType.ACTION:
                        _actionResolver.Resolve(actionId);
                        _afterActionGuid = _nextGuids[0];
                        return;
                    
                    case DNodeType.SET_VARIABLE:
                        _variablesContainer.Set(variableId, variableSet);
                        guid = _nextGuids[0];
                        break;
                    case DNodeType.BRANCH:
                        _variablesContainer.Get(variableId, out var variable);
                        guid = _nextGuids[variable ? 0 : 1];
                        break;
                }
            }
            
            _dialogueView.Hide();
            Expose();
                
            OnQuit?.Invoke();
        }

        private void Choose(int chooseIndex) => Resolve(_nextGuids[chooseIndex]);

        private void NextAfterAction() => Resolve(_afterActionGuid);
    }
}