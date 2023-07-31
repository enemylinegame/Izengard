namespace StartupMenu
{
    public interface ISettingsData
    {
        public int ResolutionWidth { get; }
        public int ResolutionHeight { get; }
        public int ShadowId { get; }

        public bool IsFullScreenOn { get; }
        public bool IsVSyncOn { get; }

        public float MasterVolumeValue { get; }
        public float MusicVolumeValue { get; }
        public float VoiceVolumeValue { get; }
        public float EffectsVolumeValue { get; }
    }
}
