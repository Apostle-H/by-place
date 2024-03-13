using System;
using Sound;
using UnityEngine;
using VContainer;

namespace Movement
{
    public class StepsSound : MonoBehaviour
    {
        [SerializeField] private AMover mover;
        [SerializeField] private AudioClip stepVFX;
        [SerializeField] private float stepsFrequency;

        private AudioPlayer _audioPlayer;

        private bool _doSteps;
        private float _stepsFrequencyCounter;

        [Inject]
        private void Inject(AudioPlayer audioPlayer) => _audioPlayer = audioPlayer;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind()
        {
            mover.OnDeparted += StartSteps;
            mover.OnStopped += StopSteps;
        }

        private void Expose()
        {
            mover.OnDeparted -= StartSteps;
            mover.OnStopped -= StopSteps;
        }

        private void StartSteps() => _doSteps = true;

        private void StopSteps(bool _) => _doSteps = false;

        private void Update()
        {
            if (!_doSteps)
            {
                if (_stepsFrequencyCounter > 0f)
                    _stepsFrequencyCounter = 0f;
                return;
            }

            _stepsFrequencyCounter += Time.deltaTime;
            if (_stepsFrequencyCounter < stepsFrequency)
                return;

            _stepsFrequencyCounter = 0f;
            
            _audioPlayer.PlayEffect(stepVFX);
        }
    }
}