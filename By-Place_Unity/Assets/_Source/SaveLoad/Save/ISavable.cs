namespace SaveLoad.Save
{
    public interface ISavable<T> : IPathable
    {
        T GetSaveData();
    }
}