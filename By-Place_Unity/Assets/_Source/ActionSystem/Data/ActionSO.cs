using UnityEngine;

namespace ActionSystem.Data
{
    [CreateAssetMenu(menuName = "SO/DS/ActionSO", fileName = "NewActionSO")]
    public class ActionSO : ScriptableObject
    {
        public int Id => GetInstanceID();
    }
}