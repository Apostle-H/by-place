using ActionSystem;
using UnityEngine;

namespace QuestSystem.Actions.Data
{
    [CreateAssetMenu(menuName = "SO/QuestSystem/Actions/FinishQuestActionSO", fileName = "NewFinishQuestActionSO")]
    public class FinishQuestActionSO : QuestActionSO
    {
        [field: SerializeField, TextArea] public string Result { get; private set; }
        
        public override IAction Build() => new FinishQuestAction(Id, QuestId, Result);
    }
}