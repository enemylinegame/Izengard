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

        private SettingsModel _gameSettingsModel;

        public SettingsModel GameSttingsModel => _gameSettingsModel;
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

            _gameSettingsModel = CreateSettignsModel(_dataManager);
      
            SaveGameSettings();
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
            SubscribeModel(model);
            
            model.SetData(initData);

            return model;
        }

        private void SubscribeModel(SettingsModel model)
        {
            model.OnSettingsChanged += ChangeSettings;
        }

        private void UnsubscribeModel(SettingsModel model)
        {
            model.OnSettingsChanged -= ChangeSettings;
        }

        private void ChangeSettings(GameSettingsType settingsType, ISettingsData senderData)
        {
            switch (settingsType) 
            {
                default:
                    break;
                case GameSettingsType.Resolution:
                case GameSettingsType.ShadowQuality:
                case GameSettingsType.FullScreenMode:
                case GameSettingsType.VSyncMode:
                    {
                        Screen.SetResolution(senderData.ResolutionWidth, senderData.ResolutionHeight, senderData.IsFullScreenOn);

                        QualitySettings.shadowResolution = (ShadowResolution)senderData.ShadowId;

                        Screen.fullScreen = senderData.IsFullScreenOn;
                        QualitySettings.vSyncCount = senderData.IsVSyncOn ? 1 : 0;
                        break;
                    }
                case GameSettingsType.MasterVolume:
                case GameSettingsType.MusicVolume:
                case GameSettingsType.VoiceVolume:
                case GameSettingsType.EffectsVolume:
                    {
                        _audioMixer.SetFloat("MasterVolume", senderData.MasterVolumeValue);
                        _audioMixer.SetFloat("MusicVolume", senderData.MusicVolumeValue);
                        _audioMixer.SetFloat("VoiceVolume", senderData.VoiceVolumeValue);
                        _audioMixer.SetFloat("EffectsVolume", senderData.EffectsVolumeValue);
                        break;
                    }
            }
        
        }

        public void SaveGameSettings()
        {
            _dataManager.SaveData(ConvertToSaveData(_gameSettingsModel));
        }

        public void RestoreGameSettings()
        {
            _gameSettingsModel.SetData(_baseSettingsData);

            _dataManager.SaveData(ConvertToSaveData(_gameSettingsModel));
        }

        public void LoadGameSettings()
        {
            var loadData = _dataManager.LoadData();

            _gameSettingsModel.SetData(loadData);
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

            UnsubscribeModel(_gameSettingsModel);
        }

        #endregion
    }
}
