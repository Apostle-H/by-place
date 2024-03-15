using Identity.Data;
using Registration;

namespace Animate.Animatable
{
    public interface IAnimatable : IIdentity
    {
        void Resolve(int stateHash);
    }
}