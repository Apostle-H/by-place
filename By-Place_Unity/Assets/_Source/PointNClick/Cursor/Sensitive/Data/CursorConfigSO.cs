using UnityEngine;

namespace PointNClick.Cursor.Sensitive.Data
{
    [CreateAssetMenu(menuName = "SO/PointNClick/Cursor/ConfigSO", fileName = "NewCursorConfigSO")]
    public class CursorConfigSO : ScriptableObject
    {
        public int Id => GetInstanceID();
        
        [field: SerializeField] public Texture2D Cursor { get; private set; }
        [field: SerializeField] public Vector2 HotSpot { get; private set; }
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public bool Capture { get; private set; }
    }
}