using ActionSystem;
using QuestSystem.Actions.Data;
using UnityEngine;
using VContainer;

namespace QuestSystem.Actions.Triggers
{
    public class ColliderTriggerQuestAction : MonoBehaviour
    {
        [SerializeField] private QuestActionSO targetQuestActionSO;

        private ActionResolver _actionResolver;
        
        [Inject]
        private void Inject(ActionResolver actionResolver) => _actionResolver = actionResolver;

        private void OnTriggerEnter(Collider other)
        {
            _actionResolver.Resolve(targetQuestActionSO.Id);
            
            gameObject.SetActive(false);
        }
    }
}