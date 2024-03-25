using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utils.Extensions
{
    public static class VisualElementExtensions
    {
        public static void SetPosition(this VisualElement visualElement, Vector2 position, LengthUnit lengthUnit = LengthUnit.Pixel)
        {
            var xTranslate = new Length(position.x, lengthUnit);
            var yTranslate = new Length(position.y, lengthUnit);
            visualElement.style.translate = new Translate(xTranslate, yTranslate);
        }

        public static void Show(this VisualElement visualElement) => 
            visualElement.style.display = DisplayStyle.Flex;
        
        public static void Hide(this VisualElement visualElement) => 
            visualElement.style.display = DisplayStyle.None;

        public static void FadeDefault(this VisualElement visualElement)
        {
            visualElement.RemoveFromClassList("fade-in");
            visualElement.RemoveFromClassList("fade-out");
        }
        
        public static void Fade(this VisualElement visualElement, bool inOut)
        {
            if (inOut)
                visualElement.FadeIn();
            else
                visualElement.FadeOut();
        }
        
        public static void FadeOut(this VisualElement visualElement)
        {
            visualElement.RemoveFromClassList("fade-in");
            visualElement.AddToClassList("fade-out");
        }

        public static void FadeIn(this VisualElement visualElement)
        {
            visualElement.RemoveFromClassList("fade-out");
            visualElement.AddToClassList("fade-in");
        }
    }
}