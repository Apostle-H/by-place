using System;
using UnityEngine;

namespace Utils.CustomAttributes
{
    public class AssetToPathAttribute : PropertyAttribute
    {
        public Type TargetType { get; private set; }
        
        public AssetToPathAttribute(Type type)
        {
            TargetType = type;
        }
    }
}