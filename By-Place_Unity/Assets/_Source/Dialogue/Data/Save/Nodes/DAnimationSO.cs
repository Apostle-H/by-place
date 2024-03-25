using Identity.Data;
using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public class DAnimationSO : DNodeSO
    {
        [field: SerializeField] public AIdentitySO IdentitySO { get; set; }
        [field: SerializeField] public AnimationClip Animation { get; set; }
    }
}