using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace QuestSystem.Data
{
    public class Quest
    {
        private List<string> _conclusions = new();
        
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Task { get; private set; }
        public ReadOnlyCollection<string> Conclusions { get; private set; }
        public string NextConclusion { get; private set; }
        public string Result { get; private set; }
        
        public Quest(int id, string title, string task, string conclusion)
        {
            Id = id;
            Title = title;
            Task = task;
            NextConclusion = conclusion;
            
            Conclusions = new ReadOnlyCollection<string>(_conclusions);
        }

        [JsonConstructor]
        private Quest(int id, string title, string task, List<string> conclusions, string nextConclusion, string result)
        {
            Id = id;
            Title = title;
            Task = task;
            _conclusions = conclusions;
            NextConclusion = nextConclusion;
            Result = result;

            Conclusions = new ReadOnlyCollection<string>(_conclusions);
        }
        
        public void Update(string task, string conclusion)
        {
            Task = task;
            _conclusions.Add(NextConclusion);
            NextConclusion = conclusion;
        }

        public void Close(string result) => Result = result;
    }
}