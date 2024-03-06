using Journal.Quest.View;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.CustomAttributes;

namespace QuestSystem.View.Data
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/View/Config", fileName = "QuestManagerViewConfig")]
    public class QuestManagerViewConfigSO : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset QuestVisualTree { get; private set; }
    }
}