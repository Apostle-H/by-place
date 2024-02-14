﻿using System;
using DialogueSystem.ActionSystem;
using DialogueSystem.ActionSystem.Data;
using UnityEngine;

namespace QuestSystem.Actions
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/Actions/QuestActionSO", fileName = "NewQuestActionSO")]
    public class QuestActionSO : ActionSO
    {
        [SerializeField] private QuestSO targetQuestSO;
        
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Task { get; private set; }

        public int QuestId => targetQuestSO.Id;
    }
}