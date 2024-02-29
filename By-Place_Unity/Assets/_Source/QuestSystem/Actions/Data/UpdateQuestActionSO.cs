using ActionSystem;
using UnityEngine;

namespace QuestSystem.Actions.Data
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/Actions/UpdateQuestActionSO", fileName = "NewUpdateQuestActionSO")]
    public class UpdateQuestActionSO : QuestActionSO
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField, TextArea] public string Task { get; private set; }
        [field: SerializeField, TextArea] public string Conclusion { get; private set; }
        
        public override IAction Build() => new UpdateQuestAction(Id, QuestId, Title, Task, Conclusion);
    }
}