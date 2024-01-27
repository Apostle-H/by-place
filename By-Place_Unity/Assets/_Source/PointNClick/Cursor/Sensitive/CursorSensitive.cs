using System;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Cursor.Sensitive
{
    public class CursorSensitive : MonoBehaviour, ICursorSensitive, IStartable, IDisposable, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CursorConfigSO configSO;

        private ICursorManager _cursorManager;

        public int Id => configSO.Id;
        public Texture2D Cursor => configSO.Cursor;
        public Vector2 HotSpot => configSO.HotSpot;
        public string Text => configSO.Text;
        
        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnQuit;

        [Inject]
        public void Inject(ICursorManager cursorManager) => _cursorManager = cursorManager;

        void IStartable.Start() => _cursorManager.AddSensitive(this);

        public void Dispose() => _cursorManager.RemoveSensitive(this);

        public void OnPointerEnter(PointerEventData eventData) => OnEnter?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => OnQuit?.Invoke(this);
    }
}