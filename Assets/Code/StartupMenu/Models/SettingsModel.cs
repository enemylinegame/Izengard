using System;

namespace StartupMenu
{
    public class SettingsModel : ISettingsData
    {
        public event Action<GameSettingsType, ISettingsData> OnSettingsChanged;

        private int _resolutionWidth;
        private int _resolutionHeight;

        private int _shadowId;

        private bool _isFullScreenOn = true;
        private bool _isVSyncOn = true;

        private float _mixerMaxValue;
        private float _mixerMinValue;

        private float _masterVolumeValue;
        private float _musicVolumeValue;
        private float _voiceVolumeValue;
        private float _effectsVolumeValue;

        #region Public ISettingsData fields

        public int ResolutionWidth 
        { 
            get => _resolutionWidth;
            private set => _resolutionWidth = value; 
        }

        public int ResolutionHeight 
        { 
            get => _resolutionHeight; 
            private set => _resolutionHeight = value; 
        }

        public int ShadowId
        {
            get => _shadowId;
            private set
            {
                _shadowId = value;
                OnSettingsChanged?.Invoke(GameSettingsType.ShadowQuality, this);
            }
        }

        public bool IsFullScreenOn
        {
            get => _isFullScreenOn;
            private set
            {
                _isFullScreenOn = value;
                OnSettingsChanged?.Invoke(GameSettingsType.FullScreenMode, this);
            }
        }

        public bool IsVSyncOn
        {
            get => _isVSyncOn;
            private set
            {
                _isVSyncOn = value;
                OnSettingsChanged?.Invoke(GameSettingsType.VSyncMode, this);
            }
        }

        public float MixerMaxValue 
        {
            get => _mixerMaxValue;
            private set
            {
                _mixerMinValue = value;
            }
        }

        public float MixerMinValue
        {
            get => _mixerMinValue;
            private set
            {
                _mixerMinValue = value;
            }
        }


        public float MasterVolumeValue
        {
            get => _masterVolumeValue;
            private set
            {
                _masterVolumeValue = value;
                OnSettingsChanged?.Invoke(GameSettingsType.MasterVolume, this);
            }
        }

        public float MusicVolumeValue
        {
            get => _musicVolumeValue;
            private set
            {
                _musicVolumeValue = value;
                OnSettingsChanged?.Invoke(GameSettingsType.MusicVolume, this);
            }
        }

        public float VoiceVolumeValue
        {
            get => _voiceVolumeValue;
            private set
            {
                _voiceVolumeValue = value;
                OnSettingsChanged?.Invoke(GameSettingsType.VoiceVolume, this);
            }
        }

        public float EffectsVolumeValue
        {
            get => _effectsVolumeValue;
            private set
            {
                _effectsVolumeValue = value;
                OnSettingsChanged?.Invoke(GameSettingsType.EffectsVolume, this);
            }
        }

        #endregion

        public SettingsModel() { }

        public void SetData(ISettingsData data)
        {
            ChangeResolution(data.ResolutionWidth, data.ResolutionHeight);

            ShadowId = data.ShadowId;

            IsFullScreenOn = data.IsFullScreenOn;
            IsVSyncOn = data.IsVSyncOn;

            MixerMaxValue = data.MixerMaxValue;
            MixerMinValue = data.MixerMinValue;

            MasterVolumeValue = data.MasterVolumeValue;
            MusicVolumeValue = data.MusicVolumeValue;
            VoiceVolumeValue = data.VoiceVolumeValue;
            EffectsVolumeValue = data.EffectsVolumeValue;
        }

        public void ChangeResolution(int newWidth, int newHeight) 
        {
            ResolutionWidth = newWidth;
            ResolutionHeight = newHeight;
            OnSettingsChanged?.Invoke(GameSettingsType.Resolution, this);
        }

        public void ChangeShadow(int newShadowId) =>
            ShadowId = newShadowId;

        public void ChangeFullScreenMode(bool state) =>
           IsFullScreenOn = state;

        public void ChangeVSyncMode(bool state) =>
           IsVSyncOn = state;

        public void ChangeMasterVolume(float volume) =>
            MasterVolumeValue = volume;

        public void ChangeMusicVolume(float volume) =>
            MusicVolumeValue = volume;

        public void ChangeVoiceVolume(float volume) => 
            VoiceVolumeValue = volume;

        public void ChangeEffectsVolume(float volume) => 
            EffectsVolumeValue = volume;
    }
}
