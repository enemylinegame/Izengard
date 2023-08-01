namespace StartupMenu.DataManager
{
    public abstract class SettingsDataManager : IDataManager<SaveLoadSettingsModel>
    {
        public bool IsDataStored { get; protected set; }

        public abstract void SaveData(SaveLoadSettingsModel data);

        public abstract SaveLoadSettingsModel LoadData();
    }
}
