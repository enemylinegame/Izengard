namespace StartupMenu
{
    public interface IDataManager<T>
    {
        bool IsDataStored { get; }

        void SaveData(T data);

        T LoadData();
    }
}
