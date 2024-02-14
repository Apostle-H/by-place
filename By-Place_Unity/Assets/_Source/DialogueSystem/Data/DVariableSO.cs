using UnityEngine;

namespace DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "SO/DS/VariableSO", fileName = "NewVariableSO")]
    public class DVariableSO : ScriptableObject
    {
        [field: SerializeField] public bool Obtained { get; set; }

        public int Id => GetInstanceID();
    }
}