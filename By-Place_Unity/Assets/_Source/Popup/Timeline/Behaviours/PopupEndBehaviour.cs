using Identity.Data;
using UnityEngine.Playables;
using VContainer;

namespace Popup.Timeline
{
    public class PopupEndBehaviour : PlayableBehaviour
    {
        private PopupManagersResolver _popupManagersResolver;
        private bool _played;
        
        [Inject]
        private void Inject(PopupManagersResolver popupManagersResolver) => _popupManagersResolver = popupManagersResolver;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;

            var identitySO = playerData as AIdentitySO;
            if (_popupManagersResolver == default || !_popupManagersResolver.TryGet(identitySO.Id, out var manager))
                return;

            manager.End();

            _played = true;
        }
    }
}