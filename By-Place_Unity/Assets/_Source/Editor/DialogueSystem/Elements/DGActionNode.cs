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
        public ActionSO TargetSO { get; set; }
        
        private ObjectField _actionTargetField;
        
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* TITLE CONTAINER */

            _actionTargetField = new ObjectField { objectType = typeof(ActionSO) };
            _actionTargetField.RegisterValueChangedCallback(evt => 
                UpdateTarget(evt.newValue == default ? GetDefaultAction() : (ActionSO)evt.newValue));
            
            titleContainer.Insert(1, _actionTargetField);
            UpdateTarget(TargetSO == default ? GetDefaultAction() : TargetSO);

            /* OUTPUT CONTAINER */
            
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = DEFAULT_OUTPUT_PORT_NAME;
            outputPort.userData = NextGuids[^1];
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();
        }

        private void UpdateTarget(ActionSO newSO)
        {
            TargetSO = newSO;
            
            _actionTargetField.value = TargetSO;
        }

        private ActionSO GetDefaultAction() => 
            (ActionSO)EditorGUIUtility.Load("DialogueSystem/DefaultAction.asset");
    }
}