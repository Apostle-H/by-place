using System.Collections.Generic;
using UnityEngine;

namespace Core.Loaders
{
    public class ResourcesLoader : ILoader<Object>
    {
        public T Load<T>(string path) where T : Object => (T)Resources.Load(path);
        
        public IEnumerable<T> LoadAll<T>(string path) where T : Object => Resources.LoadAll<T>(path);

        public void Unload(Object @object) => Resources.UnloadAsset(@object);
        
        public void Unload(IEnumerable<Object> objects)
        {
            foreach (var @object in objects)
                Unload(@object);
        }
    }
}