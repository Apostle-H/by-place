using UnityEngine;
using UnityEngine.UIElements;

namespace Journal.Quest.View.Data
{
    [CreateAssetMenu(menuName = "SO/Journal/View/Quest/PageViewConfigSO", fileName = "NewQuestPageViewConfigSO")]
    public class QuestPageViewConfigSO : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset QuestDropDown { get; private set; }
    }
}