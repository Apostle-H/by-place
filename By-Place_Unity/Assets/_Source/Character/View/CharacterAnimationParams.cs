using System;
using System.Linq;
using Character.Data;
using Movement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Character.View
{
    public class CharacterAnimationParams : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private ICharacterMover _characterMover;

        private readonly int _speedParamId = Animator.StringToHash("speed");
        
        [Inject]
        private void Inject(ICharacterMover characterMover) => _characterMover = characterMover;

        public void Start() => Bind();

        public void OnDestroy() => Expose();

        private void Bind() => _characterMover.OnSpeedUpdate += UpdateSpeed;

        private void Expose() => _characterMover.OnSpeedUpdate -= UpdateSpeed;

        private void UpdateSpeed(float speed) => animator.SetFloat(_speedParamId, speed);
    }
}