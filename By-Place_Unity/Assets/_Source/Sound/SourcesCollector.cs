using System.Diagnostics;
using UnityEngine;

namespace Sound
{
    public class SourcesCollector : MonoBehaviour
    {
        [field: SerializeField] public AudioSource MusicSource { get; private set; }
        [field: SerializeField] public AudioSource VFXSource { get; private set; }
    }
}