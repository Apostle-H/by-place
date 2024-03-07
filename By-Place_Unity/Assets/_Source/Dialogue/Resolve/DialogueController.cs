using System;
using System.Collections.Generic;
using ActionSystem;
using Animate;
using Animate.Resolve;
using Dialogue.Data;
using Dialogue.Data.NodeParams;
using Dialogue.Data.Save;
using Dialogue.Resolve.Data;
using VContainer;

namespace Dialogue.Resolve
{
    public class DialogueController
    {
        private DialogueData _dialogueData;
        private DialogueView _dialogueView;
        
        private ActionResolver _actionResolver;
        private DVariablesContainer _variablesContainer;
        private AnimationResolver _animationResolver;
        
        private int _afterActionGuid;
        
        private DDialogue _dialogue = new();
        private DAnimation _animation = new();
        private List<int> _nextGuids = new();
        
        public event Action OnQuit;

        [Inject]
        private void Inject(DialogueData dialogueData, DialogueView dialogueView, 
            ActionResolver actionResolver, DVariablesContainer variablesContainer, AnimationResolver animationResolver)
        {
            _dialogueData = dialogueData;
            _dialogueView = dialogueView;
            
            _actionResolver = actionResolver;
            _variablesContainer = variablesContainer;
            _animationResolver = animationResolver;
        }

        public void Load(DGroupSO dsGroup)
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
                switch (_dialogueData.Choose(guid, ref _dialogue, ref _animation, ref actionId, 
                            ref variableId, ref variableSet, ref _nextGuids))
                {
                    case DNodeType.DIALOGUE:
                        _dialogueView.DisplayDialogue(_dialogue);
                        _animationResolver.Resolve(_animation.AnimatableId, _animation.AnimationStateHash);
                        return;
                    case DNodeType.ACTION:
                        _afterActionGuid = _nextGuids[0];
                        _actionResolver.Resolve(actionId);   
                        return;
                    case DNodeType.ANIMATION:
                        guid = _nextGuids[0];
                        _animationResolver.Resolve(_animation.AnimatableId, _animation.AnimationStateHash);
                        break;
                    case DNodeType.SET_VARIABLE:
                        guid = _nextGuids[0];
                        _variablesContainer.Set(variableId, variableSet);
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