using Identity.Data;
using UnityEngine;

namespace Animate.Data
{
    public interface IAnimatable : IIdentity
    {
        void PlayClip(int stateHash);
    }
}