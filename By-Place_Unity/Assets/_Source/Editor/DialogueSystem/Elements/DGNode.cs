using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Utils;
using DialogueSystem.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    public class DGNode : Node
    {
        public static readonly string DEFAULT_INPUT_PORT_NAME = "IN";
        public static readonly string DEFAULT_OUTPUT_PORT_NAME = "OUT";

        public int Guid { get; set; } = -1;
        public int GroupGuid { get; set; } = -1;
        public List<DOutputData> NextGuids { get; set; } = new();
        
        protected DSGraphView graphView;

        public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            Guid = IDGenerator.NewId();
            
            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* INPUT CONTAINER */

            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = DEFAULT_INPUT_PORT_NAME;

            inputContainer.Add(inputPort);
        }
        
        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
        }
        
        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts() => DisconnectPorts(inputContainer);

        private void DisconnectOutputPorts() => DisconnectPorts(outputContainer);

        private void DisconnectPorts(VisualElement container)
        {
            foreach (var visualElement in container.Children())
            {
                var port = (Port)visualElement;
                if (!port.connected)
                    continue;

                graphView.DeleteElements(port.connections);
            }
        }
    }
}