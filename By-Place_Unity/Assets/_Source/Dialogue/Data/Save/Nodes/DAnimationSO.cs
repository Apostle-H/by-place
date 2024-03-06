using Animate.Data;
using UnityEngine;

namespace Dialogue.Data.Save.Nodes
{
    public class DAnimationSO : DNodeSO
    {
        [field: SerializeField] public AAnimatableLink AnimatableLink { get; set; }
        [field: SerializeField] public AnimationClip Animation { get; set; }
    }
}