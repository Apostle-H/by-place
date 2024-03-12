using System;
using Registration;

namespace ActionSystem
{
    public interface IAction : IRegistratable
    {
        bool Resolvable { get; }

        event Action<IAction> OnFinished;
    }
}