using System;
using PointNClick.Cursor.Manager;
using PointNClick.Movement.Data;
using UnityEngine;
using UnityEngine.AI;
using Utils.Extensions;
using VContainer;
using VContainer.Unity;

namespace PointNClick.Movement
{
    public class NavMeshMover : MonoBehaviour, IInitializable, IMover
    {
        [SerializeField] private NavMeshAgent navMeshAgent;

        private MoverConfigSO _configSO;
        private ICursorManager _cursorManager;
        
        private bool _hasTarget;
        private Vector3 _targetPos;

        public float Speed => _configSO.Speed;
        
        public event Action OnArrived;
        
        [Inject]
        private void Inject(MoverConfigSO configSO) => _configSO = configSO;

        public void Initialize() => navMeshAgent.speed = Speed;

        public void Move(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
            _hasTarget = true;
            _targetPos = target;
        }

        public void Stop()
        {
            if (!_hasTarget)
                return;
            
            navMeshAgent.ResetPath();
            _hasTarget = false;
        }

        private void FixedUpdate()
        {
            if (!_hasTarget || (transform.position.ReplaceY(_targetPos) - _targetPos).magnitude > .05f)
                return;
            
            _hasTarget = false;
            OnArrived?.Invoke();
        }
    }
}
