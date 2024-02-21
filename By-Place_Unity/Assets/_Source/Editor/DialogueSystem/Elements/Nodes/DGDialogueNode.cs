using System.Collections.Generic;
using DialogueSystem.Data;
using DialogueSystem.Data.NodeParams;
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

        public DSGraphView GraphView { get; set; }
        public DSpeakerSO SpeakerSO { get; set; }
        public List<DChoice> Choices { get; set; } = new();
        public List<DText> Texts { get; set; } = new();
        
        private ObjectField _speakerField;
        private Foldout _textsFoldout;
        
        public DGDialogueNode(Vector2 position, int guid = -1) : base(position, guid)
        {
            NextGuids.Add(new DOutputData());
            
            Texts.Add(new DText() { Text = "Text" });
            Choices.Add(new DChoice() { Text = NEW_CHOICE_TEXT });
        }

        public override void Draw()
        {
            base.Draw();
            
            /* MAIN CONTAINER */
            
            _speakerField = new ObjectField { objectType = typeof(DSpeakerSO) };
            _speakerField.RegisterValueChangedCallback(evt => 
                UpdateSpeaker(evt.newValue == default ? GetDefaultSpeaker() : (DSpeakerSO)evt.newValue));
            
            mainContainer.Insert(1, _speakerField);
            UpdateSpeaker(SpeakerSO == default ? GetDefaultSpeaker() : SpeakerSO);

            /* OUTPUT CONTAINER */

            for (var i = 0; i < NextGuids.Count; i++)
            {
                var choicePort = CreateChoicePort(i);
                outputContainer.Add(choicePort);
            }
            
            /* EXTENSION CONTAINER */

            var addChoiceButton = new Button() { text = "Add Choice" };
            addChoiceButton.clicked += AddChoice;
            addChoiceButton.AddToClassList("ds-node__button");
            
            var customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            _textsFoldout = new Foldout() { text = "Dialogue Texts" };
            var addTextButton = new Button() { text = "Add Text" };
            addTextButton.clicked += AddText;
            
            _textsFoldout.Add(addTextButton);

            for (var i = 0; i < Texts.Count; i++)
            {
                var textFoldout = CreateDialogueText(i);
                _textsFoldout.Add(textFoldout);
            }
            
            customDataContainer.Add(_textsFoldout);
            extensionContainer.Add(addChoiceButton);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
        
        public void SetColor(Color color) => mainContainer.style.backgroundColor = color;

        private Port CreateChoicePort(int index)
        {
            var outputData = NextGuids[index];
            var choice = Choices[index];
            
            var choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = DEFAULT_OUTPUT_PORT_NAME;
            choicePort.userData = NextGuids[index];

            var deleteChoiceButton = new Button() { text = "X" };
            deleteChoiceButton.clicked += () => DeleteChoicePort(choicePort, outputData, choice);
            deleteChoiceButton.AddToClassList("ds-node__button");

            var choiceTextField = new TextField() { value = choice.Text };
            choiceTextField.RegisterValueChangedCallback(evt => choice.Text = evt.newValue);
            choiceTextField.AddClasses("ds-node__text-field", "ds-node__text-field__hidden", "ds-node__choice-text-field");

            var variableBox = new Box();
            
            var checkVariableField = new ObjectField() { objectType = typeof(DVariableSO), value = choice.CheckVariableSO };
            checkVariableField.RegisterValueChangedCallback(evt =>
                UpdateVariable(choice, (DVariableSO)evt.newValue));
            checkVariableField.style.maxWidth = new Length(150, LengthUnit.Pixel);

            var checkVariableExpectedValue = new Toggle() { value = choice.ExpectedValue };
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
        
        private void UpdateSpeaker(DSpeakerSO newSpeakerSO)
        {
            SpeakerSO = newSpeakerSO;
            
            _speakerField.value = SpeakerSO;
            title = SpeakerSO.Name;
            SetColor(SpeakerSO.NodeColor);
        }

        private DSpeakerSO GetDefaultSpeaker() =>
            (DSpeakerSO)EditorGUIUtility.Load("DialogueSystem/DefaultSpeaker.asset");

        private void AddChoice()
        {
            NextGuids.Add(new DOutputData());
            Choices.Add(new DChoice() { Text = NEW_CHOICE_TEXT });

            var choicePort = CreateChoicePort(NextGuids.Count - 1);
            outputContainer.Add(choicePort);
        }

        private void UpdateVariable(DChoice choice, DVariableSO newValue) => choice.CheckVariableSO = newValue;
        
        private void UpdateVariableExpectedValue(DChoice choice, bool newValue) => choice.ExpectedValue = newValue;
        
        private void DeleteChoicePort(Port choicePort, DOutputData outputData, DChoice choice)
        {
            if (NextGuids.Count == 1)
                return;

            if (choicePort.connected)
                GraphView.DeleteElements(choicePort.connections);

            NextGuids.Remove(outputData);
            Choices.Remove(choice);
                
            GraphView.RemoveElement(choicePort);
        }

        private void AddText()
        {
            Texts.Add(new DText() { Text = "Text" });

            var textFoldout = CreateDialogueText(Texts.Count - 1);
            _textsFoldout.Add(textFoldout);
        }
        
        private Foldout CreateDialogueText(int index)
        {
            var text = Texts[index];
            var textFoldout = new Foldout() { text = "Text" };
            
            var textVariableField = new ObjectField() { objectType = typeof(DVariableSO), value = Texts[index].VariableSO};
            textVariableField.RegisterValueChangedCallback(evt => 
                UpdateTextVariable(text, (DVariableSO)evt.newValue));

            var textField = new TextField() { value = text.Text };
            textField.RegisterValueChangedCallback(evt => UpdateText(textFoldout, text, evt.newValue));
            textField.AddClasses("ds-node__text-field", "ds-node__quote-text-field");

            var deleteButton = new Button() { text = "Remove" };
            deleteButton.clicked += () => DeleteText(textFoldout, text);

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