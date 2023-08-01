using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using StartupMenu.DataManager;
using System;

namespace StartupMenu
{
    public class GameSettingsManager : IDisposable
    {
        private readonly AudioMixer _audioMixer;
        private readonly SettingsDataManager _dataManager;
        private readonly ISettingsData _baseSettingsData;

        private readonly List<Resolution> _resolutionList = new List<Resolution>();
        private readonly int _currentRefreshRate;

        private SettingsModel _model;

        public SettingsModel Model => _model;
        public List<Resolution> ResolutionList => _resolutionList;

        public GameSettingsManager(
            AudioMixer audioMixer, 
            ISettingsData baseSettingsData) 
        {
            _audioMixer = audioMixer;
            _baseSettingsData = baseSettingsData;

            _dataManager = new XMLSettingsDataManager();

            _currentRefreshRate = Screen.currentResolution.refreshRate;

            foreach (var resolution in Screen.resolutions)
            {
                if (resolution.refreshRate == _currentRefreshRate)
                {
                    _resolutionList.Add(resolution);
                }
            }

            _model = CreateSettignsModel(_dataManager);
      
            ChangeGraphicsSettings(SettingsType.Graphics);
            ChangeSoundSettings(SettingsType.Sound);

            ApplyCurrentSettings();
        }

        private SettingsModel CreateSettignsModel(SettingsDataManager dataManager)
        {
            ISettingsData initData;
            
            if (dataManager.IsDataStored == true)
            {
                initData = dataManager.LoadData();
            }
            else
            {
                initData = _baseSettingsData;
            }

            var model = new SettingsModel();
            model.SetBaseData(initData);
            SubscribeModel(model);
            
            return model;
        }

        private void SubscribeModel(SettingsModel model)
        {
            model.OnSettingsChanged += ChangeGraphicsSettings;
            model.OnSettingsChanged += ChangeSoundSettings;
        }

        private void UnsubscribeModel(SettingsModel model)
        {
            model.OnSettingsChanged -= ChangeGraphicsSettings;
            model.OnSettingsChanged -= ChangeSoundSettings;
        }

        private void ChangeGraphicsSettings(SettingsType type)
        {
            if (type != SettingsType.Graphics)
                return;

            Screen.SetResolution(
                _model.ResolutionWidth,
                _model.ResolutionHeight, 
                _model.IsFullScreenOn);

            QualitySettings.shadowResolution = (ShadowResolution)_model.ShadowId;

            Screen.fullScreen = _model.IsFullScreenOn;
            QualitySettings.vSyncCount = _model.IsVSyncOn ? 1 : 0;
        }

        private void ChangeSoundSettings(SettingsType type)
        {
            if (type != SettingsType.Sound)
                return;

            _audioMixer.SetFloat("MasterVolume", _model.MasterVolumeValue);
            _audioMixer.SetFloat("MusicVolume", _model.MusicVolumeValue);
            _audioMixer.SetFloat("VoiceVolume", _model.VoiceVolumeValue);
            _audioMixer.SetFloat("EffectsVolume", _model.EffectsVolumeValue);
        }

        public void ApplyCurrentSettings()
        {
            _dataManager.SaveData(ConvertToSaveData(_model));
        }

        public void RestoreDefaultSettings()
        {
            _model.SetBaseData(_baseSettingsData);
            _dataManager.SaveData(ConvertToSaveData(_model));

            ChangeGraphicsSettings(SettingsType.Graphics);
            ChangeSoundSettings(SettingsType.Sound);
        }

        public void CancelSettings()
        {
            var loadData = _dataManager.LoadData();
            _model.SetBaseData(loadData);

            ChangeGraphicsSettings(SettingsType.Graphics);
            ChangeSoundSettings(SettingsType.Sound);
        }

        private SaveLoadSettingsModel ConvertToSaveData(SettingsModel model)
        {
            var saveData = new SaveLoadSettingsModel
            {
                ResolutionWidth = model.ResolutionWidth,
                ResolutionHeight = model.ResolutionHeight,
                ShadowId = model.ShadowId,
                IsFullScreenOn = model.IsFullScreenOn,
                IsVSyncOn = model.IsVSyncOn,
                MasterVolumeValue = model.MasterVolumeValue,
                MusicVolumeValue = model.MusicVolumeValue,
                VoiceVolumeValue = model.VoiceVolumeValue,
                EffectsVolumeValue = model.EffectsVolumeValue
            };

            return saveData;
        }

        #region IDisposable

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            UnsubscribeModel(_model);
        }

        #endregion
    }
}
