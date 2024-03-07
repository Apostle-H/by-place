using Identity.Data;

namespace Animate.Animatable
{
    public interface IAnimatable : IIdentity
    {
        void PlayClip(int stateHash);
    }
}