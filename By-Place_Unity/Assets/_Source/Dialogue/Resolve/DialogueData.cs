using System;
using System.Collections.Generic;
using System.Linq;
using Dialogue.Data;
using Dialogue.Data.NodeParams;
using Dialogue.Data.Save;
using Dialogue.Data.Save.Nodes;
using Dialogue.Resolve.Data;
using UnityEngine;
using VContainer;

namespace Dialogue.Resolve
{
    public class DialogueData
    {
        private DContainerSO _container;

        private DVariablesContainer _variablesContainer;

        [Inject]
        private void Inject(DVariablesContainer variablesContainer) => _variablesContainer = variablesContainer;

        public void Load(DContainerSO container) => _container = container;

        public DNodeType Choose(int guid, ref DDialogue dialogue, ref DAnimation animation, ref AudioClip audioClip,
            ref int actionId, ref int variableId, ref bool variableSet, ref List<int> nextGuids)
        {
            var targetNodeSO = _container.Nodes.First(node => node.Guid == guid);
            nextGuids.Clear();

            switch (targetNodeSO)
            {
                case DDialogueSO dialogueSO:
                    ConvertDialogue(dialogueSO, ref dialogue, ref animation, ref nextGuids);
                    return DNodeType.DIALOGUE;
                case DActionSO actionSO:
                    ConvertAction(actionSO, ref actionId, ref nextGuids);
                    return DNodeType.ACTION;
                case DAnimationSO animationSO:
                    ConvertAnimation(animationSO, ref animation, ref nextGuids);
                    return DNodeType.ANIMATION;
                case DSoundSO soundSO:
                    ConvertSound(soundSO, ref audioClip, ref nextGuids);
                    return DNodeType.SOUND;
                case DSetVariableSO setVariableSO:
                    ConvertSetVariable(setVariableSO, ref variableId, ref variableSet, ref nextGuids);
                    return DNodeType.SET_VARIABLE;
                case DBranchSO branchSO:
                    ConvertBranch(branchSO, ref variableId, ref nextGuids);
                    return DNodeType.BRANCH;
                default:
                    throw new NotImplementedException("Unexpected Node Type");
            }
        }

        private void ConvertDialogue(DDialogueSO nodeSO, ref DDialogue dialogue, ref DAnimation animation, ref List<int> nextGuids)
        {
            dialogue.SpeakerName = nodeSO.SpeakerSO.Name;
            dialogue.SpeakerIcon = nodeSO.SpeakerSO.Icon;

            animation.AnimatableId = int.MaxValue;
            for (var i = 0; i < nodeSO.Texts.Count; i++)
            {
                if (i != 0 && 
                    (!_variablesContainer.Get(nodeSO.Texts[i].VariableSO.Id, out var variable) || !variable))
                    continue;

                dialogue.SpeakerText = nodeSO.Texts[i].Text;
                if (nodeSO.Texts[i].Animation == default)
                    continue;
                
                animation.AnimatableId = nodeSO.SpeakerSO.Id;
                animation.AnimationStateHash = Animator.StringToHash(nodeSO.Texts[0].Animation.name);
            }

            dialogue.Choices.Clear();
            for (var i = 0; i < nodeSO.Choices.Count; i++)
            {
                if (nodeSO.Choices[i].CheckVariableSO != default
                    && (!_variablesContainer.Get(nodeSO.Choices[i].CheckVariableSO.Id, out var variable) 
                    || variable != nodeSO.Choices[i].ExpectedValue))
                    continue;

                dialogue.Choices.Add(nodeSO.Choices[i].Text);
                nextGuids.Add(nodeSO.OutputData[i].NextGuid);
            }
        }

        private void ConvertAction(DActionSO nodeSO, ref int actionId, ref List<int> nextGuids)
        {
            actionId = nodeSO.ActionSO.Id;
            nextGuids.Add(nodeSO.OutputData[0].NextGuid);
        }

        private void ConvertAnimation(DAnimationSO nodeSO, ref DAnimation animation, ref List<int> nextGuids)
        {
            animation.AnimatableId = nodeSO.IdentitySO?.Id ?? int.MaxValue;
            animation.AnimationStateHash = Animator.StringToHash(nodeSO.Animation?.name ?? "-");
            nextGuids.Add(nodeSO.OutputData[0].NextGuid);
        }
        
        private void ConvertSound(DSoundSO nodeSO, ref AudioClip audioClip, ref List<int> nextGuids)
        {
            audioClip = nodeSO.Value;
            nextGuids.Add(nodeSO.OutputData[0].NextGuid);
        }
        
        private void ConvertSetVariable(DSetVariableSO nodeSO, ref int variableId, ref bool variableSet, 
            ref List<int> nextGuids)
        {
            variableId = nodeSO.VariableSO.Id;
            variableSet = nodeSO.SetValue;
            nextGuids.Add(nodeSO.OutputData[0].NextGuid);
        }

        private void ConvertBranch(DBranchSO nodeSO, ref int variableId, ref List<int> nextGuids)
        {
            variableId = nodeSO.VariableSO.Id;
            nextGuids.AddRange(nodeSO.OutputData.Select(nextGuid => nextGuid.NextGuid));
        }
    }
}