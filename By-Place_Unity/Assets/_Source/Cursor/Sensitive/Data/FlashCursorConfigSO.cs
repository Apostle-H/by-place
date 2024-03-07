using UnityEngine;

namespace Cursor.Sensitive.Data
{
    [CreateAssetMenu(menuName = "SO/PointNClick/Cursor/FlashConfigSO", fileName = "NewFlashCursorConfigSO")]
    public class FlashCursorConfigSO : CursorConfigSO
    {
        [field: SerializeField] public float FlashTime { get; private set; }
    }
}