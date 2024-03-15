using System;
using Sound;
using UnityEngine;
using VContainer;

namespace Movement
{
    public class StepsSound : MonoBehaviour
    {
        private AudioPlayer _audioPlayer;

        private bool _doSteps;
        private float _stepsFrequencyCounter;

        [Inject]
        private void Inject(AudioPlayer audioPlayer) => _audioPlayer = audioPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out GroundSound groundSound))
                return;
            
            _audioPlayer.PlayEffect(groundSound.StepVFX);
        }
    }
}