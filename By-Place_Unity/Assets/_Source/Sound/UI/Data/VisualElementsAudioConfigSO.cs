using UnityEngine;

namespace Sound.UI.Data
{
    [CreateAssetMenu(menuName = "SO/Sound/UI/ConfigSO", fileName = "NewVisualElementsAudioConfigSO")]
    public class VisualElementsAudioConfigSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip ClickVFX { get; private set; }
    }
}