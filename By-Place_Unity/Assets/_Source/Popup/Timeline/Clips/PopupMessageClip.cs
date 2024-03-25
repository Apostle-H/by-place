using Identity.Data;
using UnityEngine;
using UnityEngine.Playables;

namespace Popup.Timeline
{
    public class PopupMessageClip : PlayableAsset
    {
        [SerializeField, TextArea] private string text;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PopupMessageBehaviour>.Create(graph);

            var behaviour = playable.GetBehaviour();
            behaviour.Text = text;

            return playable;
        }
    }
}