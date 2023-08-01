namespace StartupMenu
{
    public class LoadSettingsModel : ISettingsData
    {
        public int ResolutionWidth { get; private set; }
        public int ResolutionHeight { get; private set; }

        public int ShadowId { get; private set; }


        public bool IsFullScreenOn { get; private set; }

        public bool IsVSyncOn { get; private set; }


        public float MixerMaxValue { get; private set; }

        public float MixerMinValue { get; private set; }


        public float MasterVolumeValue { get; private set; }

        public float MusicVolumeValue { get; private set; }

        public float VoiceVolumeValue { get; private set; }

        public float EffectsVolumeValue { get; private set; }


        public void SetResolution(int widthValue, int heightValue) 
        {
            ResolutionWidth = widthValue;
            ResolutionHeight = heightValue;
        }

        public void SetShadowId(int value) =>
            ShadowId = value;

        public void SetFullScreenMode(bool value) =>
            IsFullScreenOn = value;
        public void SetVSyncMode(bool value) => 
            IsVSyncOn = value;

        public void SetMasterVolume(float value) => 
            MasterVolumeValue = value;
        public void SetMusicVolume(float value) => 
            MusicVolumeValue = value;
        public void SetVoiceVolume(float value) => 
            VoiceVolumeValue = value;
        public void SetEffectsVolume(float value) => 
            EffectsVolumeValue = value;
    }
}
