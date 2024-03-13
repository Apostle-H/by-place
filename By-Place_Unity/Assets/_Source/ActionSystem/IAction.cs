using System;
using Identity.Data;
using Registration;

namespace ActionSystem
{
    public interface IAction : IIdentity
    {
        bool Resolvable { get; }

        event Action<IAction> OnFinished;

        void Resolve();
    }
}