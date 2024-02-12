using UnityEngine;

namespace DialogueSystem.ActionSystem.Data
{
    [CreateAssetMenu(menuName = "SO/DS/Actions/ActionSO", fileName = "NewActionTargetSO")]
    public class ActionSO : ScriptableObject
    {
        public int Id => GetInstanceID();
    }
}