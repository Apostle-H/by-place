﻿using UnityEngine;

namespace Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ReplaceXYZ(this Vector3 target, Vector3 replace, bool getX, bool getY, bool getZ)
        {
            if (getX)
                target.x = replace.x;
            if (getY)
                target.y = replace.y;
            if (getZ)
                target.z = replace.z;

            return target;
        }

        public static Vector3 ReplaceX(this Vector3 target, Vector3 replace) =>
            target.ReplaceXYZ(replace, true, false, false);
        
        public static Vector3 ReplaceY(this Vector3 target, Vector3 replace) =>
            target.ReplaceXYZ(replace, false, true, false);
        
        public static Vector3 ReplaceZ(this Vector3 target, Vector3 replace) =>
            target.ReplaceXYZ(replace, false, false, true);
    }
}