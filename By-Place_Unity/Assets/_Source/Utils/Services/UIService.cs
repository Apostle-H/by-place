using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;
using VContainer;

namespace Utils.Services
{
    public class UIService
    {
        private UIDocument _canvas;
        private InputSystemUIInputModule _inputModule;

        [Inject]
        public UIService(UIDocument canvas, InputSystemUIInputModule inputModule)
        {
            _canvas = canvas;
            _inputModule = inputModule;
        }

        public bool IsMouseOverUIElement() => IsOverUIElement(_inputModule.point.action.ReadValue<Vector2>());

        public bool IsOverUIElement(Vector2 screenPoint)
        {
            var mousePosition = ToPanelCords(screenPoint);
            
            return _canvas.rootVisualElement.panel.Pick(mousePosition) != default;
        }

        public Vector2 ToPanelCords(Vector2 screenPoint)
        {
            var inputMousePosScreen = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
            var flippedPosition = new Vector2(inputMousePosScreen.x, 1 - inputMousePosScreen.y);
            
            return flippedPosition * _canvas.rootVisualElement.panel.visualTree.layout.size;
        }
    }
}