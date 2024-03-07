using System.Collections.Generic;
using Cursor.Sensitive;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;

namespace Cursor.Manager
{
    public class UIElementsCursorManager : MonoBehaviour, ICursorManager
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private Vector2 textOffset;
        [SerializeField] private Texture2D defaultCursor;
        [SerializeField] private Vector2 defaultHotSpot;
        
        private PointNClickActions _actions;
        private Label _cursorText;

        private List<ICursorSensitive> _cursorSensitives = new();
        private int _currentCursorId;

        private bool _captured;

        [Inject]
        public void Inject(PointNClickActions actions) => _actions = actions;

        private void Awake() => _cursorText = uiDocument.rootVisualElement.Q<Label>("CursorText");

        private void Start()
        {
            _actions.Main.Point.performed += UpdateTextPos;
            
            UnityEngine.Cursor.SetCursor(defaultCursor, defaultHotSpot, CursorMode.ForceSoftware);
            _cursorText.text = string.Empty;
        }

        private void OnDestroy() => _actions.Main.Point.performed -= UpdateTextPos;

        public void AddSensitive(ICursorSensitive sensitive)
        {
            if (_cursorSensitives.Contains(sensitive)) 
                return;
            _cursorSensitives.Add(sensitive);
            
            sensitive.OnEnter += UpdateCursor;
            sensitive.OnExit += ToDefaultCursor;
        }

        public void RemoveSensitive(ICursorSensitive sensitive)
        {
            if (!_cursorSensitives.Remove(sensitive)) 
                return;
            
            sensitive.OnEnter -= UpdateCursor;
            sensitive.OnExit -= ToDefaultCursor;
        }

        private void UpdateCursor(ICursorSensitive sensitive)
        {
            if (_captured)
                return;
            
            UnityEngine.Cursor.SetCursor(sensitive.Cursor, sensitive.HotSpot, CursorMode.ForceSoftware);
            _cursorText.text = sensitive.Text;

            _currentCursorId = sensitive.Id;
            _captured = sensitive.Capture;
        }

        private void ToDefaultCursor(ICursorSensitive sensitive)
        {
            if (_currentCursorId != sensitive.Id)
                return;
            _captured = false;
            
            UnityEngine.Cursor.SetCursor(defaultCursor, defaultHotSpot, CursorMode.ForceSoftware);
            _cursorText.text = string.Empty;
        }

        private void UpdateTextPos(InputAction.CallbackContext ctx)
        {
            var inputMousePos = ctx.ReadValue<Vector2>();
            var inputMousePosScreen = new Vector2(inputMousePos.x / Screen.width, inputMousePos.y / Screen.height);
            var flippedPosition = new Vector2(inputMousePosScreen.x, 1 - inputMousePosScreen.y);
            var adjustedPosition = flippedPosition * _cursorText.parent.panel.visualTree.layout.size + textOffset;
            _cursorText.style.translate = new Translate(new Length(adjustedPosition.x, LengthUnit.Pixel),
                new Length(adjustedPosition.y, LengthUnit.Pixel));
        }
    }
}