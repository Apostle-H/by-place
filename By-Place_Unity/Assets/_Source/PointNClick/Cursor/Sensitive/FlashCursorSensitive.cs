using System;
using System.Collections;
using PointNClick.Cursor.Manager;
using PointNClick.Cursor.Sensitive.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Utils.Runners;
using VContainer;

namespace PointNClick.Cursor.Sensitive
{
    public class FlashCursorSensitive : MonoBehaviour, ICursorSensitive
    {
        [SerializeField] private FlashCursorConfigSO configSO;
        
        private ICursorManager _cursorManager;
        private CoroutineRunner _coroutineRunner;

        private WaitForSeconds _flashDelay;

        public int Id => configSO.Id;
        public Texture2D Cursor => configSO.Cursor;
        public Vector2 HotSpot => configSO.HotSpot;
        public string Text => configSO.Text;
        
        public event Action<ICursorSensitive> OnEnter;
        public event Action<ICursorSensitive> OnExit;

        [Inject]
        public void Inject(ICursorManager cursorManager, CoroutineRunner coroutineRunner)
        {
            _cursorManager = cursorManager;
            _coroutineRunner = coroutineRunner;

            _flashDelay = new WaitForSeconds(configSO.FlashTime);
        }
        
        private void Start() => _cursorManager.AddSensitive(this);

        private void OnDestroy() => _cursorManager.RemoveSensitive(this);

        public void OnMouseDown()
        {
            _coroutineRunner.StopCoroutine(Flash());
            _coroutineRunner.StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            OnEnter?.Invoke(this);
            yield return _flashDelay;
            OnExit?.Invoke(this);
        }
    }
}