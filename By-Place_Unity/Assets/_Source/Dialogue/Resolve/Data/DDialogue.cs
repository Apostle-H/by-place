using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Resolve.Data
{
    public class DDialogue
    {
        public string SpeakerName { get; set; }
        public Sprite SpeakerIcon { get; set; }
        public string SpeakerText { get; set; }
        public List<string> Choices { get; set; } = new();
    }
}