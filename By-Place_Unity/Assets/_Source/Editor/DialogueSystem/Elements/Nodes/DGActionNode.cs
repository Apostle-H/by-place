using ActionSystem.Data;
using DialogueSystem.Data.NodeParams;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements.Nodes
{
    public class DGActionNode : DGNode
    {
        public ActionSO ActionSO { get; set; }
        
        private ObjectField _actionField;

        public DGActionNode(Vector2 position, int guid = -1) : base(position, guid)
        {
            title = "Action";
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* MAIN CONTAINER */

            _actionField = new ObjectField { objectType = typeof(ActionSO) };
            _actionField.RegisterValueChangedCallback(evt => UpdateAction((ActionSO)evt.newValue));
            
            mainContainer.Insert(1, _actionField);
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