using ActionSystem;
using UnityEngine;

namespace QuestSystem.Actions.Data
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/Actions/StartQuestActionSO", fileName = "NewStartQuestActionSO")]
    public class StartQuestActionSO : QuestActionSO
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField, TextArea] public string Task { get; private set; }
        [field: SerializeField, TextArea] public string Conclusion { get; private set; }
        
        public override IAction Build()
        {
            return new StartQuestAction(Id, QuestId, Title, Task, Conclusion);
        }
    }
}