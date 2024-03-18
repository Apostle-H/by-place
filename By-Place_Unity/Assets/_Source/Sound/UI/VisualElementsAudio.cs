using System.Collections.Generic;
using Registration;
using Sound.UI.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sound.UI
{
    public class VisualElementsAudio : IRegistrator<VisualElement>
    {
        private AudioPlayer _audioPlayer;
        private AudioClip _clickVFX;
        
        private Dictionary<int, VisualElement> _visualElements = new();

        public VisualElementsAudio(AudioPlayer audioPlayer, VisualElementsAudioConfigSO configSO)
        {
            _audioPlayer = audioPlayer;
            _clickVFX = configSO.ClickVFX;
        }

        private void BindElement(VisualElement element) => element.RegisterCallback<MouseDownEvent>(PlayClickSound);

        private void ExposeElement(VisualElement element) => element.UnregisterCallback<MouseDownEvent>(PlayClickSound);

        public void Register(VisualElement registratable)
        {
            var hasCode = registratable.GetHashCode();
            if (_visualElements.ContainsKey(hasCode))
                return;
            
            _visualElements.Add(hasCode, registratable);
            
            BindElement(registratable);
        }

        public void Unregister(VisualElement registratable)
        {
            var hasCode = registratable.GetHashCode();
            if (!_visualElements.ContainsKey(hasCode))
                return;
            
            _visualElements.Remove(hasCode);
            
            ExposeElement(registratable);
        }

        private void PlayClickSound(MouseDownEvent evt) => _audioPlayer.PlayEffect(_clickVFX);
    }
}