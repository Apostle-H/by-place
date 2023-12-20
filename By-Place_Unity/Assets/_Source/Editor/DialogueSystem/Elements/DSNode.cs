using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Utilities;
using DialogueSystem.Utils;
using DialogueSystem.Utils.Extensions;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements
{
    public class DSNode : Node
    {
        public static readonly string DEFAULT_INPUT_PORT_NAME = "IN";
        public static readonly string DEFAULT_OUTPUT_PORT_NAME = "OUT";

        public GUID Guid { get; set; } = new();
        public List<DSNodeChoiceSave> Choices { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        public DialogueSpeakerSO SpeakerSO { get; set; }

        protected DSGraphView graphView;

        private ObjectField _speakerField;
        
        public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            Guid = GUID.Generate();

            Choices = new List<DSNodeChoiceSave>();
            Text = "Dialogue text.";

            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            _speakerField = new ObjectField { objectType = typeof(DialogueSpeakerSO) };
            _speakerField.RegisterValueChangedCallback(evt =>
            {
                UpdateSpeaker(evt.newValue == default ? GetDefaultSpeaker() : (DialogueSpeakerSO)evt.newValue);
                // if (Group == null)
                // {
                //     graphView.RemoveUngroupedNode(this);
                //     graphView.AddUngroupedNode(this);
                //     return;
                // }
                //
                // var currentGroup = Group;
                // graphView.RemoveGroupedNode(this, Group);
                // graphView.AddGroupedNode(this, currentGroup);
            });
            
            titleContainer.Insert(1, _speakerField);
            UpdateSpeaker(SpeakerSO == default ? GetDefaultSpeaker() : SpeakerSO);

            /* INPUT CONTAINER */

            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = DEFAULT_INPUT_PORT_NAME;

            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */

            var customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            var textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            var textTextField = DSElementUtility.CreateTextArea(Text, null, 
                callback => Text = callback.newValue);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        private void UpdateSpeaker(DialogueSpeakerSO newSpeakerSO)
        {
            SpeakerSO = newSpeakerSO;
            
            _speakerField.value = SpeakerSO;
            title = SpeakerSO.Name;
            SetColor(SpeakerSO.NodeColor);
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

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();
            return !inputPort.connected;
        }

        private DialogueSpeakerSO GetDefaultSpeaker() =>
            (DialogueSpeakerSO)EditorGUIUtility.Load("DialogueSystem/DefaultSpeaker.asset");

        public void SetColor(Color color) => mainContainer.style.backgroundColor = color;

    }
}