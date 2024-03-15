using Sound;
using UnityEngine;
using VContainer;

namespace Navigation.Location.Sound
{
    public class LocationMusicChanger : MonoBehaviour
    {
        [SerializeField] private ALocation location;
        [SerializeField] private AudioClip music;

        private AudioPlayer _audioPlayer;

        [Inject]
        private void Inject(AudioPlayer audioPlayer) => _audioPlayer = audioPlayer;
        
        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => location.OnEnter += ChangeMusic;

        private void Expose() => location.OnExit += ChangeMusic;

        private void ChangeMusic() => _audioPlayer.PlayMusic(music);
    }
}