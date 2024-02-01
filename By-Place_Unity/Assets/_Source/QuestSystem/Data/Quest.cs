using System;
using DialogueSystem.ActionSystem;
using UnityEngine;

namespace QuestSystem
{
    public class Quest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Task { get; private set; }

        public event Action OnUpdate;

        public Quest(int id, string title, string task)
        {
            Id = id;
            Title = title;
            Task = task;
        }

        public void Update(string title, string task)
        {
            Title = title;
            Task = task;
            
            OnUpdate?.Invoke();
        }
    }
}