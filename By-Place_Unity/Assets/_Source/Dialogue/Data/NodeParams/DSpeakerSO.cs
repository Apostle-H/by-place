using UnityEngine;

namespace DialogueSystem.Data.NodeParams
{
    [CreateAssetMenu(menuName = "SO/DS/SpeakerSO", fileName = "NewDSpeakerSO")]
    public class DSpeakerSO : ScriptableObject
    {
        [field: SerializeField] public int Id => GetInstanceID();
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon  { get; private set; }

#if UNITY_EDITOR
        [field: SerializeField] public Color NodeColor { get; private set; }
#endif
    }
}