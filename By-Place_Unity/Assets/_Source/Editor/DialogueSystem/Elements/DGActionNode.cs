using DialogueSystem.ActionSystem.Data;
using DialogueSystem.Data;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    public class DGActionNode : DGNode
    {
        public ActionSO ActionSO { get; set; }
        
        private ObjectField _actionField;
        
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* TITLE CONTAINER */

            _actionField = new ObjectField { objectType = typeof(ActionSO) };
            _actionField.RegisterValueChangedCallback(evt => UpdateAction((ActionSO)evt.newValue));
            
            titleContainer.Insert(1, _actionField);
            UpdateAction(ActionSO);

            /* OUTPUT CONTAINER */
            
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = DEFAULT_OUTPUT_PORT_NAME;
            outputPort.userData = NextGuids[^1];
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();
        }

        private void UpdateAction(ActionSO newSO)
        {
            ActionSO = newSO;
            
            _actionField.value = ActionSO;
        }
    }
}