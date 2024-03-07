using Cursor.Sensitive.Data;
using UnityEngine;

namespace Journal.View.Data
{
    [CreateAssetMenu(menuName = "SO/Journal/View/ConfigSO", fileName = "NewJournalViewConfigSO")]
    public class JournalViewConfigSO : ScriptableObject
    {
        [field: SerializeField] public CursorConfigSO ToggleCursorConfigSO { get; private set; }
        [field: SerializeField] public Sprite OpenedSprite { get; private set; }
        [field: SerializeField] public Sprite ClosedSprite { get; private set; }
    }
}