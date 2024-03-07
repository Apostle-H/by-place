using System;
using Cursor.Manager;
using Cursor.Sensitive.Data;
using UnityEngine;
using VContainer;

namespace Cursor.Sensitive
{
    public class CursorSensitive : MonoBehaviour, ICursorSensitive
    {
        [SerializeField] private CursorConfigSO configSO;
        
        private ICursorManager _cursorManager;

        public int Id => configSO.Id;
        public Texture2D Cursor => configSO.Cursor;
        public Vector2 HotSpot => configSO.HotSpot;
        public string Text => configSO.Text;
        public bool Capture => configSO.Capture;

        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnExit;

        [Inject]
        public void Inject(ICursorManager cursorManager) => _cursorManager = cursorManager;

        private void Start() => _cursorManager.AddSensitive(this);

        private void OnDestroy() => _cursorManager.RemoveSensitive(this);

        public void OnMouseEnter() => OnEnter?.Invoke(this);

        private void OnMouseExit() => OnExit?.Invoke(this);
    }
}