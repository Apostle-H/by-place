using Identity.Data;
using UnityEngine;

namespace Animate.Data
{
    public abstract class AAnimatableLink : ScriptableObject, IIdentity
    {
        public abstract int Id { get; }
    }
}