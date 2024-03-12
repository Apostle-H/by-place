using System.Collections.Generic;
using System.Linq;
using Dialogue.Data.NodeParams;
using Dialogue.Data.Save.Nodes;
using DialogueSystem.Utils;
using DialogueSystem.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements.Nodes
{
    public abstract class DNode : Node
    {
        public static readonly string DEFAULT_INPUT_PORT_NAME = "IN";
        public static readonly string DEFAULT_OUTPUT_PORT_NAME = "OUT";

        public int Guid { get; private set; }
        public int GroupGuid { get; set; } = -1;
        public List<DOutputData> NextGuids { get; set; } = new();

        public int RuntimeAssetId { get; set; } = int.MaxValue;

        public DNode(Vector2 position, int guid = -1)
        {
            Guid = guid == -1 ? IDGenerator.NewId() : guid;
            
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public abstract DNode NewAt(Vector2 position);

        public virtual void Draw()
        {
            /* INPUT CONTAINER */

            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = DEFAULT_INPUT_PORT_NAME;

            inputContainer.Add(inputPort);
        }
        
        public bool IsStartingNode() => !((Port)inputContainer.Children().First()).connected;

        public void DisconnectAllPorts(DGraphView graphView)
        {
            DisconnectPorts(graphView, inputContainer);
            DisconnectPorts(graphView, outputContainer);
        }

        private void DisconnectPorts(DGraphView graphView, VisualElement container)
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