using System;

namespace StartupMenu
{
    public enum SettingsType
    {
        None, 
        Graphics,
        Sound
    }

    public class SettingsModel : ISettingsData
    {
        public event Action<SettingsType> OnSettingsChanged;

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
            set => _resolutionWidth = value; 
        }

        public int ResolutionHeight 
        { 
            get => _resolutionHeight; 
            set => _resolutionHeight = value; 
        }

        public int ShadowId
        {
            get => _shadowId;
            private set
            {
                _shadowId = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
            }
        }

        public bool IsFullScreenOn
        {
            get => _isFullScreenOn;
            private set
            {
                _isFullScreenOn = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
            }
        }

        public bool IsVSyncOn
        {
            get => _isVSyncOn;
            private set
            {
                _isVSyncOn = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
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
                OnSettingsChanged?.Invoke(SettingsType.Sound);
            }
        }

        public float MusicVolumeValue
        {
            get => _musicVolumeValue;
            private set
            {
                _musicVolumeValue = value;
                OnSettingsChanged?.Invoke(SettingsType.Sound);
            }
        }

        public float VoiceVolumeValue
        {
            get => _voiceVolumeValue;
            private set
            {
                _voiceVolumeValue = value;
                OnSettingsChanged?.Invoke(SettingsType.Sound);
            }
        }

        public float EffectsVolumeValue
        {
            get => _effectsVolumeValue;
            private set
            {
                _effectsVolumeValue = value;
                OnSettingsChanged?.Invoke(SettingsType.Sound);
            }
        }

 

        #endregion

        public SettingsModel()
        {
          
        }

        public void SetBaseData(ISettingsData data)
        {
            _resolutionWidth = data.ResolutionWidth;
            _resolutionHeight = data.ResolutionHeight;

            _shadowId = data.ShadowId;

            _isFullScreenOn = data.IsFullScreenOn;
            _isVSyncOn = data.IsVSyncOn;

            _mixerMaxValue = data.MixerMaxValue;
            _mixerMinValue = data.MixerMinValue;

            _masterVolumeValue = data.MasterVolumeValue;
            _musicVolumeValue = data.MusicVolumeValue;
            _voiceVolumeValue = data.VoiceVolumeValue;
            _effectsVolumeValue = data.EffectsVolumeValue;
        }

        public void ChangeResolution(int newWidth, int newHeight) 
        {
            ResolutionWidth = newWidth;
            ResolutionHeight = newHeight;
            OnSettingsChanged?.Invoke(SettingsType.Graphics);
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
