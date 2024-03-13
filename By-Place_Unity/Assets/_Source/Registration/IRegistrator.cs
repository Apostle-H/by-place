namespace Registration
{
    public interface IRegistrator<T>
    {
        void Register(T registratable);
        void Unregister(T registratable);
    }
}