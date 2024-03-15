using VContainer;
using VContainer.Unity;

namespace Navigation.Location
{
    public class LocationsManager : IPostStartable
    {
        private StartLocationProvider _startLocationProvider;
        private LocationsProvider _locationsProvider;
        
        private ALocation _currentLocation;

        [Inject]
        public LocationsManager(LocationsProvider locationsProvider, StartLocationProvider startLocationProvider)
        {
            _startLocationProvider = startLocationProvider;
            _locationsProvider = locationsProvider;
        }

        public void PostStart() => Change(_startLocationProvider.StartLocation.Id);
        
        public void Change(int id)
        {
            if (!_locationsProvider.GetLocation(id, out var location))
                return;
            
            _currentLocation?.Exit();
            _currentLocation = location;
            _currentLocation?.Enter();
        }
    }
}