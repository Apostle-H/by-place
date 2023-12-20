using System;
using UnityEngine.UIElements;

namespace DialogueSystem.Utils
{
    public static class DSElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null) => new(onClick) { text = text };

        public static Foldout CreateFoldout(string title, bool collapsed = false) => 
            new()
            {
                text = title,
                value = !collapsed
            };

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
                textField.RegisterValueChangedCallback(onValueChanged);

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;
            
            return textArea;
        }
    }
}