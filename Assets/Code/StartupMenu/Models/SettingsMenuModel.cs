using System;

namespace StartupMenu
{
    public enum SettingsType
    {
        None, 
        Graphics,
        Sound
    }

    public class SettingsMenuModel
    {
        public event Action<SettingsType> OnSettingsChanged;


        private int _currentResolutionId;
        private int _currentGraphicsId;
        private int _currentShadowId;

        private bool _isFullScreenOn;
        private bool _isFVSyncOn;
        private bool _isBlurnOn;

        private float _masterVolumeValue;
        private float _musicVolumeValue;
        private float _voiceVolumeValue;
        private float _effectsVolumeValue;

        #region Public fields

        public int CurrentResolutionId
        {
            get => _currentResolutionId;
            private set
            {
                _currentResolutionId = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
            }
        }

        public int CurrentGraphicsId
        {
            get => _currentGraphicsId;
            private set
            {
                _currentGraphicsId = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
            }
        }

        public int CurrentShadowId
        {
            get => _currentShadowId;
            private set
            {
                _currentShadowId = value;
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

        public bool IsFVSyncOn
        {
            get => _isFVSyncOn;
            private set
            {
                _isFVSyncOn = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
            }
        }

        public bool IsBlurnOn
        {
            get => _isBlurnOn;
            private set
            {
                _isBlurnOn = value;
                OnSettingsChanged?.Invoke(SettingsType.Graphics);
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

        public void ChangeResolution(int newResolution) =>
            CurrentResolutionId = newResolution;

        public void ChangeGraphics(int newGraphicsId) =>
            CurrentGraphicsId = newGraphicsId;

        public void ChangeShadow(int newShadowId) =>
            CurrentShadowId = newShadowId;

        public void ChangeFullScreenMode(bool state) =>
           IsFullScreenOn = state;

        public void ChangeVSyncMode(bool state) =>
           IsFVSyncOn = state;

        public void ChangeBlurMode(bool state) =>
           IsBlurnOn = state;

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
