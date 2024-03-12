using Registration;

namespace SaveLoad.Load
{
    public interface ILoadable<T> : IPathable
    {
        void LoadSaveData(T saveData);
    }
}