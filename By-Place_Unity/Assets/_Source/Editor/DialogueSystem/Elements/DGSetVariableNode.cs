using System.Numerics;
using DialogueSystem.ActionSystem.Data;
using DialogueSystem.Data;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    public class DGSetVariableNode : DGNode
    {
        public DVariableSO VariableSO { get; set; }
        public bool SetValue { get; set; }
        
        private ObjectField _variableField;
        private Toggle _setValueToggle;

        public override void Initialize(DSGraphView dsGraphView, UnityEngine.Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* TITLE CONTAINER */

            _variableField = new ObjectField { objectType = typeof(DVariableSO) };
            _variableField.RegisterValueChangedCallback(evt => 
                UpdateVariable((DVariableSO)evt.newValue));

            _setValueToggle = new Toggle();
            _setValueToggle.RegisterValueChangedCallback(evt => UpdateSetValue(evt.newValue));
            
            titleContainer.Insert(1, _setValueToggle);
            titleContainer.Insert(1, _variableField);
            
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