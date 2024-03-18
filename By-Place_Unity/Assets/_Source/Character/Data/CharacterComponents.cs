using Character.Movement;
using Interactions;
using Movement;
using VContainer;

namespace Character.Data
{
    public class CharacterComponents
    {
        public ICharacterMover Mover { get; private set; }
        
        public IInteracter Interacter { get; private set; }
        
        public IInteractable TargetInteractable { get; set; }

        [Inject]
        public CharacterComponents(ICharacterMover mover, IInteracter interacter)
        {
            Mover = mover;
            Interacter = interacter;
        }
    }
}