using System;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Cursor.Sensitive
{
    public class MonoCursorSensitive : AMonoCursorSensitive, IStartable, IDisposable, 
        IPointerEnterHandler, IPointerExitHandler
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

        public void Dispose() => _cursorManager.RemoveSensitive(this);

        public void OnPointerEnter(PointerEventData eventData) => EnterInvoke();

        public void OnPointerExit(PointerEventData eventData) => ExitInvoke();
    }
}