using Movement;
using PointNClick.Interactions;
using VContainer;

namespace Character.Data
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