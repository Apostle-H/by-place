using DialogueSystem.Data;
using DialogueSystem.Data.Save;
using DialogueSystem.Utils;
using DialogueSystem.Utils.Extensions;
using DialogueSystem.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            DialogueType = DialogueType.MULTIPLE_CHOICE;

            var choiceData = new DSNodeChoiceSave() { Text = "New Choice" };
            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            var addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                var choiceData = new DSNodeChoiceSave() { Text = "New Choice" };
                Choices.Add(choiceData);

                var choicePort = CreateChoicePort(choiceData);
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTAINER */

            foreach (var choice in Choices)
            {
                var choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData)
        {
            var choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            choicePort.portName = DEFAULT_OUTPUT_PORT_NAME;
            choicePort.userData = userData;

            var choiceData = (DSNodeChoiceSave) userData;

            var deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1)
                    return;

                if (choicePort.connected)
                    graphView.DeleteElements(choicePort.connections);

                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            var choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, null, 
                callback => choiceData.Text = callback.newValue);

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