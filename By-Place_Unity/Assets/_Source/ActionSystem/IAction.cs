using System;
using Identity.Data;

namespace ActionSystem
{
    public interface IAction : IIdentity
    {
        bool Resolvable { get; set; }

        event Action<IAction> OnFinished;

        void Resolve();
        void Skip();
    }
}