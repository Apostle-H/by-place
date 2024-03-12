using Registration;

namespace SaveLoad.Load
{
    public interface ILoader
    {
        bool Load<T>(ILoadable<T> loadable);
    }
}