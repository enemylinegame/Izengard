using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";

        private readonly GameSettingsManager _gameSettings;
        private readonly StateModel _menuMonitor;

        private SettingsMenuView _view;

        private bool _isSettingsChanged;

        public SettingsMenuController(
            Transform placeForUI, 
            GameSettingsManager gameSettings,
            ISettingsData baseSettings,
            StateModel menuMonitor,
            AudioSource clickSource)
        {
            _gameSettings = gameSettings;
            _menuMonitor = menuMonitor;
 
            _view = LoadView(placeForUI);

            _view.Init(this, baseSettings, _gameSettings.ResolutionList, clickSource);

            _view.UpdateViewOptions(_gameSettings.Model);

            _isSettingsChanged = false;
        }
        
        private SettingsMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<SettingsMenuView>();
        }

        internal void ApplySettings()
        {
            _gameSettings.ApplyCurrentSettings();

            _isSettingsChanged = true;
        }

        internal void RestoreToDefautls()
        {
            _gameSettings.RestoreDefaultSettings();
            _view.UpdateViewOptions(_gameSettings.Model);

            _isSettingsChanged = true;
        }

        internal void BackToMenu()
        {
            if(_isSettingsChanged != true)
            {
                _gameSettings.CancelSettings();
                _view.UpdateViewOptions(_gameSettings.Model);
            }

            _menuMonitor.CurrentState = MenuState.Start;
        }

        internal void OnResolutionChange(int index)
        {
            var newResolution = _gameSettings.ResolutionList[index];
            _gameSettings.Model.ChangeResolution(newResolution.width, newResolution.height);
        }

        internal void OnShadowChange(int index)
        {
            _gameSettings.Model.ChangeShadow(index);
        }

        internal void OnFullScreenChange(bool value)
{
            _gameSettings.Model.ChangeFullScreenMode(value);
        }

        internal void OnVSyncChange(bool value)
{
            _gameSettings.Model.ChangeVSyncMode(value);
        }

        internal void OnMasterVolumeChange(float value)
        {
            _gameSettings.Model.ChangeMasterVolume(value);
        }

        internal void OnMusicVolumeChange(float value)
        {
            _gameSettings.Model.ChangeMusicVolume(value);
        }
        
        internal void OnVoiceVolumeChange(float value)
        {
            _gameSettings.Model.ChangeVoiceVolume(value);
        }

        internal void OnEffectsVolumeChange(float value)
        {
            _gameSettings.Model.ChangeEffectsVolume(value);
        }
    }
}
