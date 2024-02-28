using DialogueSystem.Data;
using DialogueSystem.Data.NodeParams;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements.Nodes
{
    public class DBranchNode : DNode
    {
        public DVariableSO VariableSO { get; set; }
        
        private ObjectField _variableField;
        
        public DBranchNode(Vector2 position, int guid = -1) : base(position, guid)
        {
            title = "Branch";
            NextGuids.Add(new DOutputData());
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* MAIN CONTAINER */

            _variableField = new ObjectField { objectType = typeof(DVariableSO) };
            _variableField.RegisterValueChangedCallback(evt => 
                UpdateVariable((DVariableSO)evt.newValue));

            mainContainer.Insert(1, _variableField);
            
            UpdateVariable(VariableSO);

            /* OUTPUT CONTAINER */

            var trueOutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            trueOutputPort.portName = "TRUE";
            trueOutputPort.userData = NextGuids[0];
            
            outputContainer.Add(trueOutputPort);
            
            var falseOutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            falseOutputPort.portName = "FALSE";
            falseOutputPort.userData = NextGuids[1];
            
            outputContainer.Add(falseOutputPort);
            
            RefreshExpandedState();
        }
        
        private void UpdateVariable(DVariableSO variableSO)
        {
            VariableSO = variableSO;
            _variableField.value = VariableSO;
        }
    }
}