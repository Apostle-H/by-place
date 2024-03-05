using System;
using UnityEngine;

namespace Journal.Quest.View
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