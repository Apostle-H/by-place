using Identity.Data;
using UnityEngine;
using UnityEngine.Playables;

namespace Popup.Timeline
{
    public class PopupEndClip : PlayableAsset
    {
        [SerializeField] private AIdentitySO identitySO;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) => 
            ScriptPlayable<PopupEndBehaviour>.Create(graph);
    }
}