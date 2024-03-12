using SaveLoad.Load;
using SaveLoad.Save;

namespace SaveLoad
{
    public interface ISavableLoadable<T> : ISavable<T>, ILoadable<T>
    {
        
    }
}