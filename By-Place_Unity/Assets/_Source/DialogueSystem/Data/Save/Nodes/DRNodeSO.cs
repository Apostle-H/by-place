﻿using System.Collections.Generic;
using DialogueSystem.Data.NodeParams;
using UnityEngine;

namespace DialogueSystem.Data.Save.Nodes
{
    public class DRNodeSO : ScriptableObject
    {
        [field: SerializeField] public int Guid { get; set; } = -1;
        [field: SerializeField] public bool IsStartingNode { get; set; }
        [field: SerializeField] public List<DOutputData> NextGuids { get; set; }
    }
}