﻿using System;
using UnityEngine;

namespace Dialogue.Data.NodeParams
{
    [Serializable]
    public class DText
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DVariableSO VariableSO { get; set; }
        [field: SerializeField] public AnimationClip Animation { get; set; }
    }
}