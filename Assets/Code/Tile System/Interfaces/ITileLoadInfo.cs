namespace Code.TileSystem
{
    /// <summary>
    /// Responsible for loading and unloading UI
    /// </summary>
    public interface ITileLoadInfo
    {
        /// <summary>
        /// Uploading tile information to the UI
        /// </summary>
        void LoadInfoToTheUI(TileView tile, TileModel model);
        /// <summary>
        /// unloading all data from the UI
        /// </summary>
        void Cancel();
    }
}