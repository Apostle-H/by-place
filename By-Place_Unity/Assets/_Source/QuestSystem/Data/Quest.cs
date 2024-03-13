using System;
using System.Collections.Generic;

namespace QuestSystem.Data
{
    public class Quest
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Task { get; private set; }
        public string Conclusion { get; private set; }
        public bool Finished { get; private set; }
        
        public Quest(int id) => Id = id;

        public void Update(string title, string task, string conclusion)
        {
            Title = title;
            Task = task;
            Conclusion = conclusion;
        }

        public void Finish() => Finished = true;
    }
}