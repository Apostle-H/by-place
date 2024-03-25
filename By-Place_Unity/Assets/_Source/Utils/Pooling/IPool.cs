namespace Utils.Pooling
{
    public interface IPool<T>
    {
        T Get();
        void Put(T item);
    }
}