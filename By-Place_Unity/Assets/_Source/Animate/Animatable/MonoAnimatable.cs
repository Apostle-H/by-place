using Animate.Data;
using Animate.Resolve;
using UnityEngine;
using VContainer;

namespace Animate.Animatable
{
    public class MonoAnimatable : MonoBehaviour, IAnimatable
    {
        [SerializeField] private AAnimatableLink animatableLink;
        [SerializeField] private Animator animator;

        private AnimationResolver _animationResolver;
        
        public int Id => animatableLink.Id;

        [Inject]
        private void Inject(AnimationResolver animationResolver) => _animationResolver = animationResolver;

        private void Start() => _animationResolver.Register(this);

        private void OnDestroy() => _animationResolver.Unregister(this);

        public void PlayClip(int stateHash) => animator.Play(stateHash);
    }
}