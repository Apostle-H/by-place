﻿using System;
using System.Linq;
using PointNClick.Movement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Character.View
{
    public class CharacterAnimationParams : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private IMover _mover;

        private int _speedParamId;
        
        [Inject]
        private void Inject(IMover mover) => _mover = mover;

        public void Awake() => _speedParamId = Animator.StringToHash("speed");

        public void Start() => Bind();

        public void OnDestroy() => Expose();

        private void Bind() => _mover.OnSpeedUpdate += UpdateSpeed;

        private void Expose() => _mover.OnSpeedUpdate -= UpdateSpeed;

        private void UpdateSpeed(float speed) => animator.SetFloat(_speedParamId, speed);
    }
}