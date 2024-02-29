using System;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace PointNClick.Cursor.Sensitive
{
    public class UICursorSensitive : ICursorSensitive
    {
        private CursorConfigSO _configSO;
        private ICursorManager _cursorManager;

        private VisualElement _targetVisualElement;

        public int Id => _configSO.Id;
        public Texture2D Cursor => _configSO.Cursor;
        public Vector2 HotSpot => _configSO.HotSpot;
        public string Text => _configSO.Text;
        public bool Capture => _configSO.Capture;
        
        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnExit;

        public UICursorSensitive(CursorConfigSO configSO, ICursorManager cursorManager, VisualElement targetVisualElement)
        {
            _configSO = configSO;
            _cursorManager = cursorManager;
            _targetVisualElement = targetVisualElement;
        }

        public void Bind()
        {
            _cursorManager.AddSensitive(this);
            
            _targetVisualElement.RegisterCallback<MouseEnterEvent>(Enter);
            _targetVisualElement.RegisterCallback<MouseLeaveEvent>(Exit);
        }

        public void Expose()
        {
            _cursorManager.RemoveSensitive(this);
            
            _targetVisualElement.UnregisterCallback<MouseEnterEvent>(Enter);
            _targetVisualElement.UnregisterCallback<MouseLeaveEvent>(Exit);
        }

        private void Enter(MouseEnterEvent evt) => OnEnter?.Invoke(this);

        private void Exit(MouseLeaveEvent evt) => OnExit?.Invoke(this);
        
        public class Factory
        {
            private ICursorManager _cursorManager;

            private IObjectResolver _container;

            [Inject]
            public Factory(ICursorManager cursorManager, IObjectResolver container)
            {
                _cursorManager = cursorManager;
                _container = container;
            }

            public UICursorSensitive Build(CursorConfigSO configSO, VisualElement targetVisualElement) =>
                new UICursorSensitive(configSO, _cursorManager, targetVisualElement);
        }
    }
}