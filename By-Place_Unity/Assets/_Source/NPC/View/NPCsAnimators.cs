using System.Collections.Generic;

namespace NPC.View
{
    public class NPCsAnimators
    {
        private Dictionary<int, NPCAnimator> _animators = new();

        public void AddAnimator(NPCAnimator animator)
        {
            if (_animators.ContainsKey(animator.Id))
                return;
            
            _animators.Add(animator.Id, animator);
        }

        public void RemoveAnimator(NPCAnimator animator)
        {
            if (!_animators.ContainsKey(animator.Id))
                return;
            
            _animators.Remove(animator.Id);
        }

        public void PlayClip(int id, string stateName)
        {
            if (!_animators.ContainsKey(id))
                return;
            
            _animators[id].PlayClip(stateName);
        }
    }
}