using Identity.Data;
using UnityEngine;

namespace Dialogue.Data.NodeParams
{
    [CreateAssetMenu(menuName = "SO/DS/SpeakerSO", fileName = "NewDSpeakerSO")]
    public class DSpeakerSO : AIdentitySO
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon  { get; private set; }

        public override int Id => GetInstanceID();
        
#if UNITY_EDITOR
        [field: SerializeField] public Color NodeColor { get; private set; }
#endif
    }
}