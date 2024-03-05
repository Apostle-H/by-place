using System.Collections.Generic;

namespace Core.Loaders
{
    public interface ILoader<TSource>
    {
        T Load<T>(string path) where T : TSource;
        IEnumerable<T> LoadAll<T>(string path) where T : TSource;
        
        void Unload(TSource @object);
        void Unload(IEnumerable<TSource> objects);
    }
}