using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace SaveLoad
{
    public class ResetSaves : IStartable, IDisposable
    {
        private UIDocument _canvas;

        private Label _resetSaves;
        
        [Inject]
        public ResetSaves(UIDocument canvas) => _canvas = canvas;

        public void Start()
        {
            _resetSaves = _canvas.rootVisualElement.Q<Label>("ResetSavesLabel");
            
            Bind();
        }

        public void Dispose() => Expose();

        private void Bind() => _resetSaves.RegisterCallback<MouseDownEvent>(Reset);

        private void Expose() => _resetSaves.UnregisterCallback<MouseDownEvent>(Reset);

        private void Reset(MouseDownEvent evt)
        {
            var saveFiles = Directory.GetFiles(Application.persistentDataPath);
            var saveDirectories = Directory.GetDirectories(Application.persistentDataPath);

            foreach (var saveFile in saveFiles)
                File.Delete(saveFile);

            foreach (var saveDirectory in saveDirectories)
                Directory.Delete(saveDirectory, true);
        }
    }
}