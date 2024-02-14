using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Utils;
using DialogueSystem.Utils.Extensions;
using DialogueSystem.Windows;
using Unity.Profiling;
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

        public List<DChoice> Choices { get; set; } = new();

        public List<DText> Texts { get; set; } = new();

        private ObjectField _speakerField;
        
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            Texts.Add(new DText() { Text = "Text" });

            NextGuids.Add(new DOutputData());
            Choices.Add(new DChoice() { Text = NEW_CHOICE_TEXT });
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

            /* MAIN CONTAINER */

            var addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                NextGuids.Add(new DOutputData());
                Choices.Add(new DChoice() { Text = NEW_CHOICE_TEXT });

                var choicePort = CreateChoicePort(NextGuids.Count - 1);
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTAINER */

            for (var i = 0; i < NextGuids.Count; i++)
            {
                var choicePort = CreateChoicePort(i);
                outputContainer.Add(choicePort);
            }
            
            /* EXTENSION CONTAINER */

            var customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            var textsFoldout = DSElementUtility.CreateFoldout("Dialogue Texts");
            var addTextButton = DSElementUtility.CreateButton("Add Text", () =>
            {
                Texts.Add(new DText() { Text = "Text" });

                var textFoldout = CreateDialogueText(Texts.Count - 1);
                textsFoldout.Add(textFoldout);
            });
            
            textsFoldout.Add(addTextButton);

            for (var i = 0; i < Texts.Count; i++)
            {
                var textFoldout = CreateDialogueText(i);
                textsFoldout.Add(textFoldout);
            }
            
            customDataContainer.Add(textsFoldout);
            extensionContainer.Add(customDataContainer);

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
            var outputData = NextGuids[index];
            var choice = Choices[index];
            
            var choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = DEFAULT_OUTPUT_PORT_NAME;
            choicePort.userData = NextGuids[index];
            
            var deleteChoiceButton = DSElementUtility.CreateButton("X", () => 
                DeleteChoicePort(choicePort, outputData, choice));

            deleteChoiceButton.AddToClassList("ds-node__button");

            var choiceTextField = DSElementUtility.CreateTextField(Choices[index].Text, null, 
                callback => Choices[index].Text = callback.newValue);

            choiceTextField.AddClasses("ds-node__text-field", "ds-node__text-field__hidden", "ds-node__choice-text-field");

            var variableBox = new Box();
            
            var checkVariableField = new ObjectField() { objectType = typeof(DVariableSO), value = Choices[index].CheckVariableSO };
            checkVariableField.RegisterValueChangedCallback(evt =>
                UpdateVariable(choice, (DVariableSO)evt.newValue));
            checkVariableField.style.maxWidth = new Length(150, LengthUnit.Pixel);

            var checkVariableExpectedValue = new Toggle() { value = Choices[index].ExpectedValue };
            checkVariableExpectedValue.RegisterValueChangedCallback(evt =>
                UpdateVariableExpectedValue(choice, evt.newValue));
            
            variableBox.Add(checkVariableField);
            variableBox.Add(checkVariableExpectedValue);
            variableBox.style.flexDirection = FlexDirection.Row;
            
            choicePort.Add(variableBox);
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }

        private void UpdateVariable(DChoice choice, DVariableSO newValue) => choice.CheckVariableSO = newValue;
        
        private void UpdateVariableExpectedValue(DChoice choice, bool newValue) => choice.ExpectedValue = newValue;
        
        private void DeleteChoicePort(Port choicePort, DOutputData outputData, DChoice choice)
        {
            if (NextGuids.Count == 1)
                return;

            if (choicePort.connected)
                graphView.DeleteElements(choicePort.connections);

            NextGuids.Remove(outputData);
            Choices.Remove(choice);
                
            graphView.RemoveElement(choicePort);
        }

        private Foldout CreateDialogueText(int index)
        {
            var text = Texts[index];
            var textFoldout = DSElementUtility.CreateFoldout("Text");
            
            var textVariableField = new ObjectField() { objectType = typeof(DVariableSO), value = Texts[index].VariableSO};
            textVariableField.RegisterValueChangedCallback(evt => 
                UpdateTextVariable(text, (DVariableSO)evt.newValue));
            
            var textField = DSElementUtility.CreateTextArea(Texts[index].Text, null, evt =>
                UpdateText(textFoldout, text, evt.newValue));
            textField.AddClasses("ds-node__text-field", "ds-node__quote-text-field");

            var deleteButton = DSElementUtility.CreateButton("Remove", () => DeleteText(textFoldout, text));

            if (index != 0)
                textFoldout.Add(textVariableField);
            textFoldout.Add(textField);
            if (index != 0)
                textFoldout.Add(deleteButton);

            return textFoldout;
        }

        private void UpdateTextVariable(DText text, DVariableSO newValue) => text.VariableSO = newValue;

        private void UpdateText(Foldout textFoldout, DText text, string newValue)
        {
            text.Text = newValue;
            textFoldout.text = newValue;
        }

        private void DeleteText(Foldout textFoldout, DText text)
        {
            if (Texts.Count == 1)
                return;

            Texts.Remove(text);

            textFoldout.parent.Remove(textFoldout);
        }
    }
}