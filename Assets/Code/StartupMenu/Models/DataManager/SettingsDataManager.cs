namespace StartupMenu.DataManager
{
    public abstract class SettingsDataManager : IDataManager<SettingsModel, ISettingsData>
    {
        public bool IsDataStored { get; protected set; }

        public abstract void SaveData(SettingsModel data);

        public abstract ISettingsData LoadData();
    }
}
