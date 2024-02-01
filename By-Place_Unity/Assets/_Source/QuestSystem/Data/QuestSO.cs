using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/QuestSO", fileName = "NewQuestSO")]
    public class QuestSO : ScriptableObject
    {
        public int Id => GetInstanceID();
    }
}