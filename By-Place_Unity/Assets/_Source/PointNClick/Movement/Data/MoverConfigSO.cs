using UnityEngine;

namespace PointNClick.Movement.Data
{
    [CreateAssetMenu(menuName = "SO/Movement/MoverConfigSO", fileName = "NewMoverConfigSO")]
    public class MoverConfigSO : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float AngleToWait { get; private set; }
    }
}