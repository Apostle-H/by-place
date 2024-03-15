using System;
using UnityEngine;
using UnityEngine.AI;
using Utils.Extensions;

namespace Movement
{
    public class NavMeshMover : AMover
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float angleToWait;
        
        private bool _hasTarget;
        private Vector3 _targetPos;

        private bool _rotating;
        private Quaternion _toRotation;
        private float _rotationValue = 0f;

        public override float Speed => navMeshAgent.speed;

        public override float CurrentSpeed { get; protected set; }

        public override event System.Action OnDeparted;
        public override event Action<bool> OnStopped;
        public override event Action<float> OnSpeedUpdate;
        
        public void Awake()
        {
            navMeshAgent.updateUpAxis = false;
        }

        public override void Move(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
            _hasTarget = true;
            _targetPos = target;
            
            _rotating = false;
            _rotationValue = 0f;
            
            OnDeparted?.Invoke();
        }

        public override void Rotate(Quaternion rotation)
        {
            _rotating = true;
            _toRotation = rotation;
        }

        public override void Stop()
        {
            if (!_hasTarget)
                return;
            
            navMeshAgent.ResetPath();
            _hasTarget = false;
            _rotating = false;
            _rotationValue = 0f;

            OnStopped?.Invoke(false);
        }

        public override void Arrive()
        {
            navMeshAgent.ResetPath();
            _hasTarget = false;
            _rotating = false;
            _rotationValue = 0f;
            
            navMeshAgent.Warp(navMeshAgent.pathEndPosition);
            OnStopped?.Invoke(true);
        }

        private void FixedUpdate()
        {
            CurrentSpeed = navMeshAgent.velocity.magnitude;
            OnSpeedUpdate?.Invoke(CurrentSpeed / Speed);

            if (_rotating)
                Rotate();
            
            if (!_hasTarget)
                return;
            
            if (!_rotating)
                CheckAngle();
            
            if (navMeshAgent.isOnOffMeshLink)
                navMeshAgent.CompleteOffMeshLink();
            
            if ((transform.position.ReplaceY(_targetPos) - _targetPos).magnitude > .01f)
                return;
            
            _hasTarget = false;
            OnStopped?.Invoke(true);
        }

        private void CheckAngle()
        {
            var direction = navMeshAgent.steeringTarget - navMeshAgent.transform.position;
            var toRotation = Quaternion.LookRotation(direction);
            if (Quaternion.Angle(navMeshAgent.transform.rotation, toRotation) < angleToWait)
                return;

            navMeshAgent.isStopped = true;
            Rotate(toRotation);
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
        }
    }
}
