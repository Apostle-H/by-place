namespace Registration
{
    public interface IRegistrator<T> where T : IRegistratable
    {
        void Register(T registratable);
        void Unregister(T registratable);
        void Resolve(int id);
    }
    
    public interface IRegistrator<TRegistratable, TParam> where TRegistratable : IRegistratable<TParam>
    {
        void Register(TRegistratable registratable);
        void Unregister(TRegistratable registratable);
        void Resolve(int id, TParam value);
    }
}