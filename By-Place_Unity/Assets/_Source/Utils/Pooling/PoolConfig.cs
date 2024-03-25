using System;

namespace Utils.Pooling
{
    public abstract class PoolConfig<T>
    {
        public abstract Func<T> Factory { get; }
        public abstract Action<T> GetCallback { get; }
        public abstract Action<T> PutCallback { get; }
    }
}