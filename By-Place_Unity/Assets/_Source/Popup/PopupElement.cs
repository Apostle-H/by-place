using System.Collections;
using UnityEngine.UIElements;

namespace Popup
{
    public class PopupElement : VisualElement
    {
        public Label TextLabel { get; private set; }
        
        public PopupElement()
        {
            name = "Popup";
            AddToClassList("popup");

            TextLabel = new Label() { name = "PopupText"};
            TextLabel.AddToClassList("title");
            Add(TextLabel);
        }
    }
}