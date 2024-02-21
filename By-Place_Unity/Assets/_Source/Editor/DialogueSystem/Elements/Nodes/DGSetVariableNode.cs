using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Data.NodeParams;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    public class DGSetVariableNode : DGNode
    {
        public DVariableSO VariableSO { get; set; }
        public bool SetValue { get; set; }
        
        private ObjectField _variableField;
        private Toggle _setValueToggle;

        public DGSetVariableNode(Vector2 position, int guid = -1) : base(position, guid)
        {
            title = "Set Variable";
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* MAIN CONTAINER */

            var variableBox = new Box();
            variableBox.style.flexDirection = FlexDirection.Row;
            
            _variableField = new ObjectField { objectType = typeof(DVariableSO) };
            _variableField.RegisterValueChangedCallback(evt => 
                UpdateVariable((DVariableSO)evt.newValue));

            _setValueToggle = new Toggle();
            _setValueToggle.RegisterValueChangedCallback(evt => UpdateSetValue(evt.newValue));
            
            variableBox.Add(_variableField);
            variableBox.Add(_setValueToggle);
            mainContainer.Insert(1, variableBox);
            
            UpdateVariable(VariableSO);
            UpdateSetValue(SetValue);

            /* OUTPUT CONTAINER */
            
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = DEFAULT_OUTPUT_PORT_NAME;
            outputPort.userData = NextGuids[^1];
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();
        }

        private void UpdateVariable(DVariableSO variableSO)
        {
            VariableSO = variableSO;
            _variableField.value = VariableSO;
        }

        private void UpdateSetValue(bool value)
        {
            SetValue = value;
            _setValueToggle.value = value;
        }
    }
}