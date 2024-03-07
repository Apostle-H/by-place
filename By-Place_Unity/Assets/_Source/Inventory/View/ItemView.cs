using System;
using Inventory.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory.View
{
    public class ItemView
    {
        private VisualElement _icon;
        
        public VisualElement Root { get; private set; }

        public ItemSO TargetItem { get; private set; }

        public event Action<ItemView> OnHover;
        public event Action<ItemView> OnUnhover;

        public ItemView(VisualElement root, ItemSO itemSO)
        {
            Root = root;

            _icon = Root.Q<VisualElement>("Icon");
            
            SetTargetItem(itemSO);
        }

        public void SetTargetItem(ItemSO itemSO)
        {
            TargetItem = itemSO;
            
            _icon.style.backgroundImage = Background.FromSprite(TargetItem.Icon);
        }

        public void Bind()
        {
            Root.RegisterCallback<MouseEnterEvent>(Hover);
            Root.RegisterCallback<MouseLeaveEvent>(Unhover);
        }

        public void Expose()
        {
            Root.UnregisterCallback<MouseEnterEvent>(Hover);
            Root.UnregisterCallback<MouseLeaveEvent>(Unhover);
        }

        public void Show() => Root.style.display = DisplayStyle.Flex;

        public void Hide() => Root.style.display = DisplayStyle.None;
        
        private void Hover(MouseEnterEvent evt) => OnHover?.Invoke(this);

        private void Unhover(MouseLeaveEvent evt) => OnUnhover?.Invoke(this);

        public void SetTransform(Vector2 translate, LengthUnit lengthUnit = LengthUnit.Pixel)
        {
            var xTranslate = new Length(translate.x, lengthUnit);
            var yTranslate = new Length(translate.y, lengthUnit);
            Root.style.translate = new Translate(xTranslate, yTranslate);
        }
    }
}