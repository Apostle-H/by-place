using System;
using Movement;
using Movement.Data;
using UnityEngine;
using UnityEngine.AI;
using Utils.Extensions;
using VContainer;

namespace Character.Data
{
    public class CharacterNavMeshMover : AMover, ICharacterMover
    {
        [SerializeField] private NavMeshAgent navMeshAgent;

        private MoverConfigSO _configSO;
        
        private bool _hasTarget;
        private Vector3 _targetPos;
        
        private bool _rotating;
        private Quaternion _toRotation;
        private float _rotationValue = 0f;

        public override float Speed => _configSO.Speed;

        public override float CurrentSpeed { get; protected set; }

        public override event System.Action OnArrived;
        public override event Action<float> OnSpeedUpdate;
        
        [Inject]
        private void Inject(MoverConfigSO configSO) => _configSO = configSO;

        public void Awake()
        {
            navMeshAgent.speed = Speed;
            navMeshAgent.angularSpeed = _configSO.AngularSpeed;

            navMeshAgent.updateUpAxis = false;
        }

        public override void Move(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
            _hasTarget = true;
            _targetPos = target;
        }

        public override void Stop()
        {
            if (!_hasTarget)
                return;
            
            navMeshAgent.ResetPath();
            _hasTarget = false;
        }
        
        private void FixedUpdate()
        {
            CurrentSpeed = navMeshAgent.velocity.magnitude;

            if (_hasTarget)
            {
                if (!_rotating)
                    CheckAngle();
                else
                    Rotate();
            }
            
            
            if (navMeshAgent.isOnOffMeshLink)
                navMeshAgent.CompleteOffMeshLink();
            
            OnSpeedUpdate?.Invoke(CurrentSpeed);
            if (!_hasTarget || (transform.position.ReplaceY(_targetPos) - _targetPos).magnitude > .05f)
                return;
            
            _hasTarget = false;
            OnArrived?.Invoke();
        }

        private void CheckAngle()
        {
            var direction = navMeshAgent.steeringTarget - navMeshAgent.transform.position;
            _toRotation = Quaternion.LookRotation(direction);
            if (Quaternion.Angle(navMeshAgent.transform.rotation, _toRotation) < _configSO.AngleToWait)
                return;

            navMeshAgent.isStopped = true;
            CurrentSpeed = Speed / 2;
            _rotating = true;
        }

        private void Rotate()
        {
            navMeshAgent.transform.rotation = Quaternion.Lerp(navMeshAgent.transform.rotation, _toRotation, _rotationValue);
            _rotationValue += Time.fixedDeltaTime;
            if (_rotationValue < .9f)
                navMeshAgent.isStopped = false;
            if (_rotationValue < 1f)
                return;

            _rotating = false;
            _rotationValue = 0f;
            
            CurrentSpeed = Speed;
        }
    }
}