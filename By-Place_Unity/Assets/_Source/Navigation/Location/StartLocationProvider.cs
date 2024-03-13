using VContainer;

namespace Navigation.Location
{
    public class StartLocationProvider
    {
        public ALocation StartLocation { get; private set; }

        [Inject]
        public StartLocationProvider(ALocation startLocation) => StartLocation = startLocation;
    }
}