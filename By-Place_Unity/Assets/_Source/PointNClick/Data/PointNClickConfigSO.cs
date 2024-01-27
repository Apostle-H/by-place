using UnityEngine;

namespace PointNClick.Data
{
    [CreateAssetMenu(menuName = "SO/PointNClick/Config", fileName = "PointNClickConfigSO")]
    public class PointNClickConfigSO : ScriptableObject
    {
        [field: SerializeField] public LayerMask WalkableMask { get; private set; } 
        [field: SerializeField] public LayerMask InteractableMask { get; private set; } 
    }
}