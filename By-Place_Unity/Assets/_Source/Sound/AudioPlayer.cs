using UnityEngine;
using VContainer;

namespace Sound
{
    public class AudioPlayer
    {
        private AudioSource _musicSource;
        private AudioSource _vfxSource;

        [Inject]
        public AudioPlayer(SourcesCollector sourcesCollector)
        {
            _musicSource = sourcesCollector.MusicSource;
            _vfxSource = sourcesCollector.VFXSource;
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
        }

        public void PlayEffect(AudioClip clip)
        {
            _vfxSource.PlayOneShot(clip);
        }
    }
}