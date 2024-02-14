using UnityEngine;

namespace Utils.Extensions
{
    public static class PhysicsExtensions
    {
        public static bool Contains(this LayerMask layerMask, int layer) => 
            layerMask == (layerMask | (1 << layer));
    }
}