namespace StartupMenu
{
    [System.Serializable]
    public class SaveLoadSettingsModel : ISettingsData
    {
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }

        public int ShadowId { get; set; }

        public bool IsFullScreenOn { get; set; }

        public bool IsVSyncOn { get; set; }

        public float MixerMaxValue { get; set; }

        public float MixerMinValue { get; set; }

        public float MasterVolumeValue { get; set; }

        public float MusicVolumeValue { get; set; }

        public float VoiceVolumeValue { get; set; }

        public float EffectsVolumeValue { get; set; }


        public SaveLoadSettingsModel() { }
    }
}
