using System.Collections.Generic;
using Registration;

namespace Popup
{
    public class PopupManagersResolver : IRegistrator<PopupManager>
    {
        private Dictionary<int, PopupManager> _popupManagers = new();
        
        public void Register(PopupManager popupManager)
        {
            if (_popupManagers.ContainsKey(popupManager.Id))
                return;
            
            _popupManagers.Add(popupManager.Id, popupManager);
        }

        public void Unregister(PopupManager popupManager) => _popupManagers.Remove(popupManager.Id);

        public bool TryGet(int id, out PopupManager popupManager)
        {
            popupManager = default;
            if (!_popupManagers.ContainsKey(id))
                return false;

            popupManager = _popupManagers[id];
            return true;
        }
    }
}