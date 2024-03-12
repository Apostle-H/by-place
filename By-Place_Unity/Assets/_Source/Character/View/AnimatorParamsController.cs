using Movement;
using UnityEngine;

namespace Character.View
{
    public class AnimatorParamsController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AMover mover;

        private readonly int _speedParamId = Animator.StringToHash("speed");
        
        public void Start() => Bind();

        public void OnDestroy() => Expose();

        private void Bind() => mover.OnSpeedUpdate += UpdateSpeed;

        private void Expose() => mover.OnSpeedUpdate -= UpdateSpeed;

        private void UpdateSpeed(float speed) => animator.SetFloat(_speedParamId, speed);
    }
}