using System;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class NavMeshMover : MonoBehaviour, IMoveable
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [field: SerializeField] public float Speed { get; private set; }

        private bool _hasTarget;
        
        public event Action OnArrived;

        private void Awake() => navMeshAgent.speed = Speed;

        public void Move(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
            _hasTarget = true;
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
            if (!_hasTarget 
                || navMeshAgent.pathPending 
                || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance 
                || (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f))
                return;
            
            OnArrived?.Invoke();
            _hasTarget = false;
        }
    }
}
