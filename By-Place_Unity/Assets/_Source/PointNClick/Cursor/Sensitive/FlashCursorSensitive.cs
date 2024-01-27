using System;
using System.Collections;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Runners;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Cursor.Sensitive
{
    public class FlashCursorSensitive : MonoBehaviour, ICursorSensitive, IStartable, IDisposable, IPointerDownHandler
    {
        [SerializeField] private FlashCursorConfigSO configSO;
        
        private ICursorManager _cursorManager;
        private CoroutineRunner _coroutineRunner;

        public int Id => configSO.Id;
        public Texture2D Cursor => configSO.Cursor;
        public Vector2 HotSpot => configSO.HotSpot;
        public string Text => configSO.Text;
        
        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnQuit;

        private WaitForSeconds _flashDelay;

        [Inject]
        public void Inject(ICursorManager cursorManager, CoroutineRunner coroutineRunner)
        {
            _cursorManager = cursorManager;
            _coroutineRunner = coroutineRunner;

            _flashDelay = new WaitForSeconds(configSO.FlashTime);
        }

        void IStartable.Start() => _cursorManager.AddSensitive(this);

        public void Dispose() => _cursorManager.RemoveSensitive(this);

        public void OnPointerDown(PointerEventData eventData)
        {
            _coroutineRunner.StopCoroutine(Flash());
            _coroutineRunner.StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            OnEnter?.Invoke(this);
            yield return _flashDelay;
            OnQuit?.Invoke(this);
        }
    }
}