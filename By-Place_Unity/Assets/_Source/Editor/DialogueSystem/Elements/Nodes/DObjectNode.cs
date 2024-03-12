using ActionSystem.Data;
using Dialogue.Data.NodeParams;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements.Nodes
{
    public abstract class DObjectNode<T> : DNode where T : Object
    {
        public T Value { get; set; }
        
        private ObjectField _objectField;

        public DObjectNode(string name, Vector2 position, int guid = -1) : base(position, guid)
        {
            title = name;
            NextGuids.Add(new DOutputData());
        }

        public override void Draw()
        {
            base.Draw();
            
            /* MAIN CONTAINER */

            _objectField = new ObjectField { objectType = typeof(T) };
            _objectField.RegisterValueChangedCallback(evt => UpdateValue((T)evt.newValue));
            
            mainContainer.Insert(1, _objectField);
            UpdateValue(Value);

            /* OUTPUT CONTAINER */
            
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = DEFAULT_OUTPUT_PORT_NAME;
            outputPort.userData = NextGuids[^1];
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();
        }

        protected virtual void UpdateValue(T newSO)
        {
            Value = newSO;
            
            _objectField.value = newSO;
        }
    }
}