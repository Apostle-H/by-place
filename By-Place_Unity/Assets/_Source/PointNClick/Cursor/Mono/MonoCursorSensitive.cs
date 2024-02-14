using System;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Cursor.Sensitive
{
    public class MonoCursorSensitive : AMonoCursorSensitive
    {
        [SerializeField] private CursorConfigSO configSO;
        
        private ICursorManager _cursorManager;

        public override int Id => configSO.Id;
        public override Texture2D Cursor => configSO.Cursor;
        public override Vector2 HotSpot => configSO.HotSpot;
        public override string Text => configSO.Text;
        
        [Inject]
        public void Inject(ICursorManager cursorManager) => _cursorManager = cursorManager;

        public void Start() => _cursorManager.AddSensitive(this);

        public void OnDestroy() => _cursorManager.RemoveSensitive(this);

        private void OnMouseEnter() => EnterInvoke();

        public void OnMouseExit() => ExitInvoke();
    }
}