using UnityEngine;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace PointNClick.Inventory.View
{
    public class ItemInfoView : IStartable
    {
        private UIDocument _canvas;

        private Label _itemNameLabel;
        private Label _itemDescriptionLabel;
        
        public VisualElement Root { get; private set; }
        
        public ItemInfoView(UIDocument canvas) => _canvas = canvas;

        public void Start()
        {
            Root = _canvas.rootVisualElement.Q<VisualElement>("ItemInfoPanel");
            
            _itemNameLabel = Root.Q<Label>("ItemNameLabel");
            _itemDescriptionLabel = Root.Q<Label>("ItemDescriptionLabel");
            
            Hide();
        }

        public void Show() => Root.style.display = DisplayStyle.Flex;
        
        public void Hide() => Root.style.display = DisplayStyle.None;

        public void UpdateInfo(string name, string description)
        {
            _itemNameLabel.text = name;
            _itemDescriptionLabel.text = description;
        }
        
        public void SetTransform(Vector2 translate, LengthUnit lengthUnit = LengthUnit.Pixel)
        {
            var xTranslate = new Length(translate.x, lengthUnit);
            var yTranslate = new Length(translate.y, lengthUnit);
            Root.style.translate = new Translate(xTranslate, yTranslate);
        }
    }
}