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
    public class FlashCursorSensitive : AMonoCursorSensitive, IPointerDownHandler
    {
        [SerializeField] private FlashCursorConfigSO configSO;
        
        private ICursorManager _cursorManager;
        private CoroutineRunner _coroutineRunner;

        private WaitForSeconds _flashDelay;

        public override int Id => configSO.Id;
        public override Texture2D Cursor => configSO.Cursor;
        public override Vector2 HotSpot => configSO.HotSpot;
        public override string Text => configSO.Text;

        [Inject]
        public void Inject(ICursorManager cursorManager, CoroutineRunner coroutineRunner)
        {
            _cursorManager = cursorManager;
            _coroutineRunner = coroutineRunner;

            _flashDelay = new WaitForSeconds(configSO.FlashTime);
        }
        
        private void Start() => _cursorManager.AddSensitive(this);

        private void OnDestroy() => _cursorManager.RemoveSensitive(this);

        public void OnPointerDown(PointerEventData eventData)
        {
            _coroutineRunner.StopCoroutine(Flash());
            _coroutineRunner.StartCoroutine(Flash());
        }
        
        private IEnumerator Flash()
        {
            EnterInvoke();
            yield return _flashDelay;
            ExitInvoke();
        }
    }
}