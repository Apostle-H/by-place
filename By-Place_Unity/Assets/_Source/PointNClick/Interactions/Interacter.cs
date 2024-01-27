using PointNClick.Data;
using PointNClick.Movement;
using UnityEngine;
using VContainer;

namespace PointNClick.Interactions
{
    public class Interacter : IInteracter
    {
        private readonly IMover _mover;

        private IInteractable _targetInteractable;

        private bool _onTheWay;

        [Inject]
        public Interacter(IMover mover) => _mover = mover;

        public void Interact(IInteractable interactable)
        {
            _targetInteractable = interactable;
            _mover.OnArrived += StartInteraction;
                
            _mover.Move(_targetInteractable.Position);
            _onTheWay = true;
        }

        public void Cancel()
        {
            if (!_onTheWay)
                return;
            
            _mover.OnArrived -= StartInteraction;
            _mover.Stop();
            _onTheWay = false;
        }

        private void StartInteraction()
        {
            _mover.OnArrived -= StartInteraction;
            _targetInteractable.Interact();
            _onTheWay = false;
        }
    }
}