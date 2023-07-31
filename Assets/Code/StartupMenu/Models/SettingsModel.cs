using System;
using System.Collections.Generic;
using UnityEngine;

namespace StartupMenu
{
    public enum SettingsType
    {
        None, 
        Graphics,
        Sound
    }

    public class SettingsModel
    {
        public event Action<SettingsType> OnSettingsChanged;

        private int _currentResolutionWidth;
        private int _currentResolutionHeight;

        private int _currentShadowId;

        private bool _isFullScreenOn = true;
        private bool _isFVSyncOn = true;

        private float _masterVolumeValue;
        private float _musicVolumeValue;
        private float _voiceVolumeValue;
        private float _effectsVolumeValue;

        #region Public fields

        public int CurrentResolutionWidth 
        { 
            get => _currentResolutionWidth; 
            set => _currentResolutionWidth = value; 
        }

        public int CurrentResolutionHeight 
        { 
            get => _currentResolutionHeight; 
            set => _currentResolutionHeight = value; 
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
            _currentResolutionWidth = data.ResolutionWidth;
            _currentResolutionHeight = data.ResolutionHeight;

            _currentShadowId = data.ShadowId;

            _isFullScreenOn = data.IsFullScreenOn;
            _isFVSyncOn = data.IsVSyncOn;

            _masterVolumeValue = data.MasterVolumeValue;
            _musicVolumeValue = data.MusicVolumeValue;
            _voiceVolumeValue = data.VoiceVolumeValue;
            _effectsVolumeValue = data.EffectsVolumeValue;
        }

        public void ChangeResolution(int newWidth, int newHeight) 
        {
            CurrentResolutionWidth = newWidth;
            CurrentResolutionHeight = newHeight;
            OnSettingsChanged?.Invoke(SettingsType.Graphics);
        }

        public void ChangeShadow(int newShadowId) =>
            CurrentShadowId = newShadowId;

        public void ChangeFullScreenMode(bool state) =>
           IsFullScreenOn = state;

        public void ChangeVSyncMode(bool state) =>
           IsFVSyncOn = state;

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
