using System;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class NavMeshMover : MonoBehaviour, IMoveable
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float speed;

        public float Speed => speed;

        private void Awake() => navMeshAgent.speed = speed;

        public void Move(Vector3 target) => navMeshAgent.SetDestination(target);

        public void Stop()
        {
            if (navMeshAgent.isStopped)
                return;
            
            navMeshAgent.ResetPath();
        }
    }
}
