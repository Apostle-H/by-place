using UnityEngine;

namespace QuestSystem.Data
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/QuestSO", fileName = "NewQuestSO")]
    public class QuestSO : ScriptableObject
    {
        public int Id => GetInstanceID();
    }
}