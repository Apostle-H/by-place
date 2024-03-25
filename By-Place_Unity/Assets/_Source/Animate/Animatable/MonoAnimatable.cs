using Animate.Resolve;
using Identity.Data;
using UnityEngine;
using VContainer;

namespace Animate.Animatable
{
    public class MonoAnimatable : MonoBehaviour, IAnimatable
    {
        [SerializeField] private AIdentitySO identitySO;
        [SerializeField] private Animator animator;

        private AnimationResolver _animationResolver;
        
        public int Id => identitySO.Id;

        [Inject]
        private void Inject(AnimationResolver animationResolver) => _animationResolver = animationResolver;

        private void Start() => _animationResolver.Register(this);

        private void OnDestroy() => _animationResolver.Unregister(this);

        public void Resolve(int stateHash) => animator.Play(stateHash);
    }
}