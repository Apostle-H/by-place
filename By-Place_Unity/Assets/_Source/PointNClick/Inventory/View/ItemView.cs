using PointNClick.Items.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace PointNClick.Items.View
{
    public class ItemView
    {
        private VisualElement _icon;
        
        public VisualElement Root { get; private set; }

        public ItemView(VisualElement root, ItemSO itemSO)
        {
            Root = root;

            _icon = Root.Q<VisualElement>("Icon");
            
            SetTargetItem(itemSO);
        }

        public void SetTargetItem(ItemSO itemSO) => _icon.style.backgroundImage = Background.FromSprite(itemSO.Icon);

        public void Show() => Root.style.display = DisplayStyle.Flex;

        public void Hide() => Root.style.display = DisplayStyle.None;

        public void SetTransform(Vector2 translate, LengthUnit lengthUnit = LengthUnit.Pixel)
        {
            var xTranslate = new Length(translate.x, lengthUnit);
            var yTranslate = new Length(translate.y, lengthUnit);
            Root.style.translate = new Translate(xTranslate, yTranslate);
        }
    }
}