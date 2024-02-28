using DialogueSystem.Data.NodeParams;
using UnityEngine;
using VContainer;

namespace NPC.View
{
    public class NPCAnimator : MonoBehaviour
    {
        [field: SerializeField] public DSpeakerSO Speaker { get; private set; }
        
        [SerializeField] private Animator animator;

        private NPCsAnimators _animators;

        public int Id => Speaker.GetInstanceID();
        
        [Inject]
        private void Inject(NPCsAnimators animators) => _animators = animators;

        private void Start() => _animators.AddAnimator(this);

        private void OnDestroy() => _animators.RemoveAnimator(this);
        
        public void PlayClip(string stateName) => animator.Play(stateName, 0);
    }
}