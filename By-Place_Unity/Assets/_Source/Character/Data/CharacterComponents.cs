using PointNClick.Interactions;
using PointNClick.Movement;
using VContainer;

namespace PointNClick.Data
{
    public class CharacterComponents
    {
        public IMover Mover { get; private set; }
        
        public IInteracter Interacter { get; private set; }
        
        public IInteractable TargetInteractable { get; set; }

        [Inject]
        public CharacterComponents(IMover mover, IInteracter interacter)
        {
            Mover = mover;
            Interacter = interacter;
        }
    }
}