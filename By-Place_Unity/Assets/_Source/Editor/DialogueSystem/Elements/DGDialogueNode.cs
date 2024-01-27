using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Data;
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
    public class DGDialogueNode : DGNode
    {
        public const string NEW_CHOICE_TEXT = "New Choice";
        
        public DSpeakerSO SpeakerSO { get; set; }
        public string Text { get; set; }

        public List<string> ChoicesTexts { get; set; } = new();

        private ObjectField _speakerField;
        
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            Text = "Dialogue text.";

            NextGuids.Add(new DOutputData());
            ChoicesTexts.Add(NEW_CHOICE_TEXT);
        }

        public override void Draw()
        {
            base.Draw();
            
            /* TITLE CONTAINER */

            _speakerField = new ObjectField { objectType = typeof(DSpeakerSO) };
            _speakerField.RegisterValueChangedCallback(evt => 
                UpdateSpeaker(evt.newValue == default ? GetDefaultSpeaker() : (DSpeakerSO)evt.newValue));
            
            titleContainer.Insert(1, _speakerField);
            UpdateSpeaker(SpeakerSO == default ? GetDefaultSpeaker() : SpeakerSO);

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

            /* MAIN CONTAINER */

            var addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                NextGuids.Add(new DOutputData());
                ChoicesTexts.Add(NEW_CHOICE_TEXT);

                var choicePort = CreateChoicePort(NextGuids.Count - 1);
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTAINER */

            for (int i = 0; i < NextGuids.Count; i++)
            {
                var choicePort = CreateChoicePort(i);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
        
        private void UpdateSpeaker(DSpeakerSO newSpeakerSO)
        {
            SpeakerSO = newSpeakerSO;
            
            _speakerField.value = SpeakerSO;
            title = SpeakerSO.Name;
            SetColor(SpeakerSO.NodeColor);
        }

        private DSpeakerSO GetDefaultSpeaker() =>
            (DSpeakerSO)EditorGUIUtility.Load("DialogueSystem/DefaultSpeaker.asset");

        public void SetColor(Color color) => mainContainer.style.backgroundColor = color;

        private Port CreateChoicePort(int index)
        {
            var choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = DEFAULT_OUTPUT_PORT_NAME;
            choicePort.userData = NextGuids[index];

            var deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
            {
                if (NextGuids.Count == 1)
                    return;

                if (choicePort.connected)
                    graphView.DeleteElements(choicePort.connections);

                NextGuids.RemoveAt(index);
                ChoicesTexts.RemoveAt(index);
                
                graphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            var choiceTextField = DSElementUtility.CreateTextField(ChoicesTexts[index], null, 
                callback => ChoicesTexts[index] = callback.newValue);

            choiceTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}