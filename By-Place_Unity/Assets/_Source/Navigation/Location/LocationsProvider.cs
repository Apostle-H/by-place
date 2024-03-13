using System.Collections.Generic;
using Registration;
using VContainer;

namespace Navigation.Location
{
    public class LocationsProvider : IRegistrator<ALocation>
    {
        private Dictionary<int, ALocation> _locations = new();
        
        public void Register(ALocation location)
        {
            if(_locations.ContainsKey(location.Id))
                return;
            
            _locations.Add(location.Id, location);
        }

        public void Unregister(ALocation location)
        {
            if (!_locations.ContainsKey(location.Id))
                return;

            _locations.Remove(location.Id);
        }

        public bool GetLocation(int id, out ALocation location)
        {
            _locations.TryGetValue(id, out location);
            
            return _locations.ContainsKey(id);
        }
    }
}