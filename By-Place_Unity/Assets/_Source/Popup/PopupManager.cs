using System;
using Identity.Data;
using Popup.Data;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Extensions;
using Utils.Pooling;
using VContainer;

namespace Popup
{
    public class PopupManager : MonoBehaviour, IIdentity
    {
        [SerializeField] private AIdentitySO identitySO;
        [SerializeField] private Transform target;

        private PopupManagersResolver _resolver;
        
        private UIDocument _canvas;

        private VisualElement _popupRoot;
        private VisualElement _popupBox;
        private DefaultPool<PopupElement> _popupsPool;

        private PopupElement _currentPopupElement;
        
        public int Id => identitySO.Id;

        [Inject]
        private void Inject(UIDocument canvas, PopupConfigSO configSO, DefaultPool<PopupElement> popupsPool, 
            PopupManagersResolver resolver)
        {
            _resolver = resolver;
            _canvas = canvas;
            
            _canvas.rootVisualElement.RegisterCallback<GeometryChangedEvent>(SetPosition);

            _popupRoot = configSO.PopupBoxTreeAsset.Instantiate().Q<VisualElement>("PopupRoot");
            _popupBox = _popupRoot.Q<VisualElement>("PopupBox");
            
            _popupsPool = popupsPool;
        }

        private void Awake() => _canvas.rootVisualElement.Add(_popupRoot);

        private void Start() => _resolver.Register(this);

        private void OnDestroy() => _resolver.Unregister(this);

        public void Popup(string message)
        {
            _currentPopupElement?.RegisterCallback<TransitionEndEvent>(Popdown);
            _currentPopupElement?.FadeOut();
            _currentPopupElement = _popupsPool.Get();
            _currentPopupElement.TextLabel.text = message;
            _popupBox.Add(_currentPopupElement);
        }

        public void End()
        {
            _currentPopupElement?.RegisterCallback<TransitionEndEvent>(Popdown);
            _currentPopupElement?.FadeOut();
        }

        private void Popdown(TransitionEndEvent evt)
        {
            var popup = evt.target as VisualElement;
            popup.UnregisterCallback<TransitionEndEvent>(Popdown);
            popup.FadeDefault();
            
            _popupBox.Remove(popup);
        }

        private void SetPosition(GeometryChangedEvent evt)
        {
            SetPosition(target.position);
            _canvas.rootVisualElement.UnregisterCallback<GeometryChangedEvent>(SetPosition);
        }
        
        private void SetPosition(Vector3 position)
        {
            var worldPos = position;
            var screenPosition = Camera.main.WorldToScreenPoint(worldPos);
            var screenPositionClamped = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
            var flippedPosition = new Vector2(screenPositionClamped.x, 1 - screenPositionClamped.y);
            var adjustedPosition = flippedPosition * _popupRoot.panel.visualTree.layout.size - 
                                   _popupRoot.layout.size.ReplaceY(Vector2.zero) / 2;
            
            _popupRoot.SetPosition(adjustedPosition);
        }
    }
}