using Identity.Data;

namespace Registration
{
    public interface IRegistratable : IIdentity
    {
        void Resolve();
    }

    public interface IRegistratable<T> : IIdentity
    {
        void Resolve(T value);
    }
}