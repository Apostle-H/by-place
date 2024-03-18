using UnityEngine;

namespace Movement.Sound
{
    public class GroundSound : MonoBehaviour
    {
        [field: SerializeField] public AudioClip StepVFX { get; private set; }
    }
}