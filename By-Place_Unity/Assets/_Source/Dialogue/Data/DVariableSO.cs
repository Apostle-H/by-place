using UnityEngine;

namespace Dialogue.Data
{
    [CreateAssetMenu(menuName = "SO/DS/VariableSO", fileName = "NewVariableSO")]
    public class DVariableSO : ScriptableObject
    {
        [field: SerializeField] public bool Value { get; set; }

        public int Id => GetInstanceID();
    }
}