using UnityEngine;

namespace PointNClick.Data
{
    [CreateAssetMenu(menuName = "SO/PointNClick/Config", fileName = "PointNClickConfigSO")]
    public class PointNClickConfigSO : ScriptableObject
    {
        [field: Header("Logic")]
        [field: SerializeField] public LayerMask WalkableMask { get; private set; } 
        [field: SerializeField] public LayerMask InteractableMask { get; private set; } 
        
        [field: Header("View")]
        [field: SerializeField] public float CursorToggleTime { get; private set; }
        [field: SerializeField] public Texture2D IdleCursor { get; private set; }
        [field: SerializeField] public Texture2D MoveCursor { get; private set; }
    }
}