using DialogueSystem.Data;
using DialogueSystem.Utilities;
using DialogueSystem.Utils.Extensions;
using DialogueSystem.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Elements
{
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);

            DialogueType = DialogueType.SINGLE_CHOICE;

            DSNodeChoiceSave nodeChoiceSaveData = new DSNodeChoiceSave() { Text = "Next" };

            Choices.Add(nodeChoiceSaveData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */

            foreach (DSNodeChoiceSave choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = DEFAULT_OUTPUT_PORT_NAME;
                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
