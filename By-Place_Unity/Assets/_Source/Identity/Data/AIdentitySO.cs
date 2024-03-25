using UnityEngine;

namespace Identity.Data
{
    public abstract class AIdentitySO : ScriptableObject, IIdentity
    {
        public abstract int Id { get; }
    }
}