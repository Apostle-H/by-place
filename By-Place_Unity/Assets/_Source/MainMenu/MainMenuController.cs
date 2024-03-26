using System;
using UnityEngine.UIElements;
using Utils.Services;
using VContainer;
using VContainer.Unity;

namespace MainMenu
{
    public class MainMenuController : IStartable, IDisposable
    {
        private MainMenuView _view;
        private SceneService _sceneService;

        [Inject]
        public MainMenuController(MainMenuView view, SceneService sceneService)
        {
            _view = view;
            _sceneService = sceneService;
        }


        public void Start() => _view.StartBtn.RegisterCallback<MouseDownEvent>(LoadNext);

        public void Dispose() => _view.StartBtn.UnregisterCallback<MouseDownEvent>(LoadNext);

        private void LoadNext(MouseDownEvent evt) => 
            _sceneService.LoadScene(_sceneService.GetActiveSceneBuildIndex() + 1);
    }
}