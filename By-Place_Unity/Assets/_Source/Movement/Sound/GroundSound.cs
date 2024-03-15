using UnityEngine;

namespace Movement
{
    public class GroundSound : MonoBehaviour
    {
        [field: SerializeField] public AudioClip StepVFX { get; private set; }
    }
}