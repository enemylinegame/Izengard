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

        private ISettingsData _deafaultSettingsData;
        private SettingsModel _model;

        public SettingsModel Model => _model;
        public List<Resolution> ResolutionList => _resolutionList;

        public GameSettingsManager(
            AudioMixer audioMixer, 
            ISettingsData baseSettingsData) 
        {
            _audioMixer = audioMixer;
            _baseSettingsData = baseSettingsData;

            _dataManager = new PlayerPrefsSettings();

            _model = CreateSettignsModel(_dataManager);
    
            _currentRefreshRate = Screen.currentResolution.refreshRate;

            foreach (var resolution in Screen.resolutions)
            {
                if(resolution.refreshRate == _currentRefreshRate)
                {
                    _resolutionList.Add(resolution);
                }
            }

            ChangeGraphicsSettings(SettingsType.Graphics);
            ChangeSoundSettings(SettingsType.Sound);
        }

        private int GetBaseResolutionIndex(int widht, int height)
        {
            var resultIndex = 0;

            var resolutions = Screen.resolutions;
            var currentRefreshRate = Screen.currentResolution.refreshRate;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == widht
                    && resolutions[i].height == height
                    && resolutions[i].refreshRate == currentRefreshRate)
                {
                    resultIndex = i;
                }
            }

            return resultIndex;
        }

        private SettingsModel CreateSettignsModel(SettingsDataManager dataManager)
        {
            if (dataManager.IsDataStored == true)
            {
                _deafaultSettingsData = dataManager.LoadData();
            }
            else
            {
                _deafaultSettingsData = _baseSettingsData;
            }

            var model = new SettingsModel();
            model.SetBaseData(_deafaultSettingsData);
            SubscribeModel(model);
            
            return model;
        }

        private void ChangeGraphicsSettings(SettingsType type)
        {
            if (type != SettingsType.Graphics)
                return;

            var currentResolution = _resolutionList[_model.CurrentResolutionId];
            Screen.SetResolution(
                currentResolution.width, 
                currentResolution.height, 
                _model.IsFullScreenOn, 
                currentResolution.refreshRate);

            QualitySettings.shadowResolution = (ShadowResolution)_model.CurrentShadowId;

            Screen.fullScreen = _model.IsFullScreenOn;
            QualitySettings.vSyncCount = _model.IsFVSyncOn ? 1 : 0;
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
            _dataManager.SaveData(_model);
            var loadData =  _dataManager.LoadData();
            _deafaultSettingsData = loadData;
        }

        public void RestoreDefaultSettings()
        {
            _model.SetBaseData(_deafaultSettingsData);

            ChangeGraphicsSettings(SettingsType.Graphics);
            ChangeSoundSettings(SettingsType.Sound);

            _dataManager.SaveData(_model);
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
