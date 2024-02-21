using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.NodeParams;
using DialogueSystem.Data.Save;
using DialogueSystem.Data.Save.Nodes;
using DialogueSystem.Resolve.Data;
using VContainer;

namespace DialogueSystem.Resolve
{
    public class DialogueData
    {
        private DRContainerSO _container;

        private DVariablesContainer _variablesContainer;

        [Inject]
        private void Inject(DVariablesContainer variablesContainer) => _variablesContainer = variablesContainer;

        public void Load(DRContainerSO container) => _container = container;

        public DNodeType Choose(int guid, ref DDialogue dialogue, ref List<int> nextGuids, ref int actionId,
            ref int variableId, ref bool variableSet)
        {
            var targetNodeSO = _container.Nodes.First(node => node.Guid == guid);
            nextGuids.Clear();

            switch (targetNodeSO)
            {
                case DRDialogueNodeSO dialogueNodeSO:
                    dialogue = ConvertDialogueNode(dialogueNodeSO, ref nextGuids);
                    return DNodeType.DIALOGUE;
                case DRActionNodeSO actionNodeSO:
                    actionId = ConvertActionNode(actionNodeSO, ref nextGuids);
                    return DNodeType.ACTION;
                case DRSetVariableNodeSO setVariableNodeSO:
                    (variableId, variableSet) = ConvertSetVariableNode(setVariableNodeSO, ref nextGuids);
                    return DNodeType.SET_VARIABLE;
                case DRBranchNodeSO branchNodeSO:
                    variableId = ConvertBranchNoe(branchNodeSO, ref nextGuids);
                    return DNodeType.BRANCH;
                default:
                    throw new NotImplementedException("Unexpected Node Type");
            }
        }

        private DDialogue ConvertDialogueNode(DRDialogueNodeSO nodeSO, ref List<int> nextGuids)
        {
            var dialogue = new DDialogue();

            dialogue.SpeakerName = nodeSO.SpeakerSO.Name;
            dialogue.SpeakerIcon = nodeSO.SpeakerSO.Icon;
            dialogue.SpeakerText = nodeSO.Texts[0].Text;
            for (var i = 1; i < nodeSO.Texts.Count; i++)
            {
                if (!_variablesContainer.Get(nodeSO.Texts[i].VariableSO.Id, out var variable)
                    || !variable)
                    continue;

                dialogue.SpeakerText = nodeSO.Texts[i].Text;
            }

            dialogue.Choices = new List<string>();
            for (var i = 0; i < nodeSO.Choices.Count; i++)
            {
                if (nodeSO.Choices[i].CheckVariableSO != default
                    && (!_variablesContainer.Get(nodeSO.Choices[i].CheckVariableSO.Id, out var variable) ||
                        variable != nodeSO.Choices[i].ExpectedValue))
                    continue;

                dialogue.Choices.Add(nodeSO.Choices[i].Text);
                nextGuids.Add(nodeSO.NextGuids[i].NextGuid);
            }

            return dialogue;
        }

        private int ConvertActionNode(DRActionNodeSO nodeSO, ref List<int> nextGuids)
        {
            nextGuids.Add(nodeSO.NextGuids[0].NextGuid);
            return nodeSO.ActionSO.Id;
        }

        private (int, bool) ConvertSetVariableNode(DRSetVariableNodeSO nodeSO, ref List<int> nextGuids)
        {
            nextGuids.Add(nodeSO.NextGuids[0].NextGuid);
            return (nodeSO.VariableSO.Id, nodeSO.SetValue);
        }

        private int ConvertBranchNoe(DRBranchNodeSO nodeSO, ref List<int> nextGuids)
        {
            nextGuids.AddRange(nodeSO.NextGuids.Select(nextGuid => nextGuid.NextGuid));
            return nodeSO.VariableSO.Id;
        }
    }
}