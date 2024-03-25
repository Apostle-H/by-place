using Identity.Data;
using UnityEngine.Timeline;

namespace Popup.Timeline
{
    [TrackBindingType(typeof(AIdentitySO))]
    [TrackClipType(typeof(PopupMessageClip))]
    [TrackClipType(typeof(PopupEndClip))]
    public class PopupTrack : TrackAsset { }
}