using DialogueSystem.ActionSystem.Data;
using UnityEngine;

namespace DialogueSystem.ActionSystem.BasicActions.Data
{
    [CreateAssetMenu(menuName = "SO/DS/Actions/OnOffGOActionSO", fileName = "NewOnOffGOActionSO")]
    public class OnOffGOActionSO : ActionSO
    {
        [field: SerializeField] public bool OffOn { get; private set; }
    }
}