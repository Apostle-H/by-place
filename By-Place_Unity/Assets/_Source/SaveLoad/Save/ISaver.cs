namespace SaveLoad.Save
{
    public interface ISaver
    {
        bool Save<T>(ISavable<T> savable);
    }
}