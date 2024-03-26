using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace MainMenu
{
    public class MainMenuView : VisualElement, IStartable
    {
        private UIDocument _canvas;
        
        public VisualElement MainMenu { get; private set; }
        public VisualElement StartBtn { get; private set; }
        public Label StartLabel { get; private set; }
        
        [Inject]
        public MainMenuView(UIDocument canvas)
        {
            _canvas = canvas;
            
            name = "MainMenuPanel";
            pickingMode = PickingMode.Ignore;
            AddToClassList("main-menu-panel");

            MainMenu = new VisualElement() { name = "MainMenu" };
            MainMenu.AddToClassList("main-menu");
            Add(MainMenu);

            StartBtn = new VisualElement() { name = "StartBtn" };
            StartBtn.AddToClassList("btn");
            MainMenu.Add(StartBtn);

            StartLabel = new Label() { name = "StartLabel", text = "Play" };
            StartLabel.AddToClassList("title");
            StartLabel.style.color = Color.yellow;
            StartBtn.Add(StartLabel);
        }

        public void Start() => _canvas.rootVisualElement.Add(this);
    }
}