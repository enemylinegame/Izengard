namespace StartupMenu
{
    public interface IDataManager<TSave, TLoad>
    {
        bool IsDataStored { get; }

        void SaveData(TSave data);

        TLoad LoadData();
    }
}
