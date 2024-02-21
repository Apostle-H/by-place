using System;
using System.Collections.Generic;
using System.Linq;
using ActionSystem.Data;
using DialogueSystem.Data.NodeParams;
using DialogueSystem.Elements;
using DialogueSystem.Windows;
using UnityEngine;

namespace DialogueSystem.Data
{
    [Serializable]
    public class DENode
    {
        [field: SerializeField] public int Guid { get; set; }
        [field: SerializeField] public int GroupGuid { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public List<DOutputData> NextGuids { get; set; }
        [field: SerializeField] public int RuntimeAssetId { get; private set; }
        
        [field: SerializeField] public DNodeType NodeType { get; set; }
        
        [field: Header("DialogueNode")]
        [field: SerializeField] public DSpeakerSO SpeakerSO { get; set; } 
        [field: SerializeField] public List<DChoice> Choices { get; set; }
        [field: SerializeField] public List<DText> Texts { get; set; }
        
        [field: Header("Action")]
        [field: SerializeField] public ActionSO ActionSO { get; set; }
        
        [field: Header("Variable")]
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public bool SetValue { get; set; }

        public void Save(DGNode node)
        {
            Guid = node.Guid;
            GroupGuid = node.GroupGuid;
            Position = node.GetPosition().position;
            NextGuids = node.NextGuids.ToList();
            RuntimeAssetId = node.RuntimeAssetId;
            
            switch (node)
            {
                case DGDialogueNode dialogueNode:
                    NodeType = DNodeType.DIALOGUE;
                    SpeakerSO = dialogueNode.SpeakerSO;
                    Choices = dialogueNode.Choices.ToList();
                    Texts = dialogueNode.Texts.ToList();
                    break;
                case DGActionNode actionNode:
                    NodeType = DNodeType.ACTION;
                    ActionSO = actionNode.ActionSO;
                    break;
                case DGSetVariableNode setVariableNode:
                    NodeType = DNodeType.SET_VARIABLE;
                    VariableSO = setVariableNode.VariableSO;
                    SetValue = setVariableNode.SetValue;
                    break;
                case DGBranchNode branchNode:
                    NodeType = DNodeType.BRANCH;
                    VariableSO = branchNode.VariableSO;
                    break;
            }
        }

        public DGNode Load(DSGraphView graphView)
        {
            DGNode node;
            switch (NodeType)
            {
                case DNodeType.DIALOGUE:
                {
                    var dialogueNode = new DGDialogueNode(Position, Guid);
                    dialogueNode.SpeakerSO = SpeakerSO;
                    dialogueNode.Choices = Choices.ToList();
                    dialogueNode.Texts = Texts.ToList();
                    dialogueNode.GraphView = graphView;

                    node = dialogueNode;
                    break;
                }
                case DNodeType.ACTION:
                {
                    var actionNode = new DGActionNode(Position, Guid);
                    actionNode.ActionSO = ActionSO;

                    node = actionNode;
                    break;
                }
                case DNodeType.SET_VARIABLE:
                {
                    var setVariableNode = new DGSetVariableNode(Position, Guid);
                    setVariableNode.VariableSO = VariableSO;
                    setVariableNode.SetValue = SetValue;

                    node = setVariableNode;
                    break;
                }
                case DNodeType.BRANCH:
                {
                    var branchNode = new DGBranchNode(Position, Guid);
                    branchNode.VariableSO = VariableSO;

                    node = branchNode;
                    break;
                }
                default:
                    throw new ArgumentException($"Unexpected node type in the save");
            }

            node.NextGuids = NextGuids.ToList();
            node.RuntimeAssetId = RuntimeAssetId;

            return node;
        }
    }
}