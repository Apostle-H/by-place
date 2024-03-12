using System.Collections.Generic;
using Animate.Animatable;
using Registration;

namespace Animate.Resolve
{
    public class AnimationResolver : IRegistrator<IAnimatable, int>
    {
        private Dictionary<int, IAnimatable> _animatables = new();

        public void Register(IAnimatable animatable)
        {
            if (_animatables.ContainsKey(animatable.Id))
                return;

            _animatables.Add(animatable.Id, animatable);
        }

        public void Unregister(IAnimatable animatable) => _animatables.Remove(animatable.Id);

        public void Resolve(int id, int stateHash)
        {
            if (!_animatables.ContainsKey(id))
                return;

            _animatables[id].Resolve(stateHash);
        }
    }
}