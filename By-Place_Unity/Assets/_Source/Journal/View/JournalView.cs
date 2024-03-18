using System;
using Cursor.Sensitive;
using Journal.View.Data;
using Sound;
using Sound.UI;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Journal.View
{
    public class JournalView : IStartable, IDisposable
    {
        private JournalViewConfigSO _configSO;
        
        private UIDocument _canvas;

        private UICursorSensitive.Factory _cursorSensitiveFactory;
        private readonly VisualElementsAudio _visualElementsAudio;
        
        private UICursorSensitive _toggleCursorSensitive;

        private VisualElement _root;

        private VisualElement _toggleBtn;
        private VisualElement _pagePanel;

        private bool _show;

        public JournalView(JournalViewConfigSO configSO, UIDocument canvas, 
            UICursorSensitive.Factory cursorSensitiveFactory, VisualElementsAudio visualElementsAudio)
        {
            _configSO = configSO;
            _canvas = canvas;

            _cursorSensitiveFactory = cursorSensitiveFactory;
            _visualElementsAudio = visualElementsAudio;
        }

        public void Start()
        {
            _root = _canvas.rootVisualElement.Q<VisualElement>("JournalPanel");

            _toggleBtn = _root.Q<VisualElement>("ToggleBtn");
            _pagePanel = _root.Q<VisualElement>("PagePanel");

            _toggleCursorSensitive = _cursorSensitiveFactory.Build(_configSO.ToggleCursorConfigSO, _toggleBtn);
            
            HidePage();
            
            Bind();
        }

        public void Dispose() => Expose();

        private void Bind()
        {
            _toggleBtn.RegisterCallback<MouseDownEvent>(ToggleItems);
            _toggleCursorSensitive.Bind();
            
            _visualElementsAudio.Register(_toggleBtn);
        }

        private void Expose()
        {
            _toggleBtn.UnregisterCallback<MouseDownEvent>(ToggleItems);
            _toggleCursorSensitive.Expose();
            
            _visualElementsAudio.Unregister(_toggleBtn);
        }

        private void ToggleItems(MouseDownEvent evt)
        {
            if (!_show)
                ShowPage();
            else
                HidePage();
        }

        private void ShowPage()
        {
            _toggleBtn.style.backgroundImage = Background.FromSprite(_configSO.OpenedSprite);
            _pagePanel.style.display = DisplayStyle.Flex;
            
            _show = true;
        }

        private void HidePage()
        {
            _toggleBtn.style.backgroundImage = Background.FromSprite(_configSO.ClosedSprite);
            _pagePanel.style.display = DisplayStyle.None;
            
            _show = false; 
        }
    }
}