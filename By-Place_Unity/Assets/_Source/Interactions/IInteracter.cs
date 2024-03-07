namespace Interactions
{
    public interface IInteracter
    {
        public void Interact(IInteractable interactable);
        public void Cancel();
    }
}