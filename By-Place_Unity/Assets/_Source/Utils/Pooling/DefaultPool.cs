using System;
using System.Collections.Generic;
using VContainer;

namespace Utils.Pooling
{
    public class DefaultPool<T> : IPool<T>
    {
        private PoolConfig<T> _config;
        
        private Queue<T> _container = new();

        [Inject]
        public DefaultPool(PoolConfig<T> config) => _config = config;

        public T Get()
        {
            if (_container.Count < 1)
                Add();

            var item = _container.Dequeue();
            _config.GetCallback(item);
            
            return item;
        }

        public void Put(T item)
        {
            _config.PutCallback(item);
            _container.Enqueue(item);
        }

        private void Add() => _container.Enqueue(_config.Factory());
    }
}