using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";

        private readonly GameSettingsManager _gameSettings;
        private readonly StateModel _menuMonitor;

        private Dictionary<SettingsMenuActionType, Action> _settingsMenuActions;
        private Dictionary<GameSettingsType, Action<object>> _changeSettingsActions;

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

            _settingsMenuActions = GetMenuActions();
            _changeSettingsActions = GetSettingsAction();

            _view = LoadView(placeForUI);

            _view.Init(
                _settingsMenuActions, 
                _changeSettingsActions, 
                baseSettings, 
                _gameSettings.ResolutionList, 
                clickSource);

            _view.UpdateViewOptions(_gameSettings.Model);

            _isSettingsChanged = false;
        }
        
        private Dictionary<SettingsMenuActionType, Action> GetMenuActions()
        {
            var actionsDictionary = new Dictionary<SettingsMenuActionType, Action>
            {
                [SettingsMenuActionType.ApplySettings] = ApplySettings,
                [SettingsMenuActionType.RestoreSettings] = RestoreToDefautls,
                [SettingsMenuActionType.BackToMenu] = BackToMenu
            };

            return actionsDictionary;
        }

        private Dictionary<GameSettingsType, Action<object>> GetSettingsAction()
        {
            var actionsDictionary = new Dictionary<GameSettingsType, Action<object>>
            {
                [GameSettingsType.Resolution] = new MethodExtension<int>(OnResolutionChange).OnChange,
                [GameSettingsType.ShadowQuality] = new MethodExtension<int>(OnShadowChange).OnChange,
                [GameSettingsType.FullScreenMode] = new MethodExtension<bool>(OnFullScreenChange).OnChange,
                [GameSettingsType.VSyncMode] = new MethodExtension<bool>(OnVSyncChange).OnChange,
                [GameSettingsType.MasterVolume] = new MethodExtension<float>(OnMasterVolumeChange).OnChange,
                [GameSettingsType.MusicVolume] = new MethodExtension<float>(OnMusicVolumeChange).OnChange,
                [GameSettingsType.VoiceVolume] = new MethodExtension<float>(OnVoiceVolumeChange).OnChange,
                [GameSettingsType.EffectsVolume] = new MethodExtension<float>(OnEffectsVolumeChange).OnChange
            };

            return actionsDictionary;
        }

        private SettingsMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<SettingsMenuView>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _changeSettingsActions.Clear();
        }

        #region Settings Menu Actions

        private void ApplySettings()
        {
            _gameSettings.ApplyCurrentSettings();

            _isSettingsChanged = true;
        }

        private void RestoreToDefautls()
        {
            _gameSettings.RestoreDefaultSettings();
            _view.UpdateViewOptions(_gameSettings.Model);

            _isSettingsChanged = true;
        }

        private void BackToMenu()
        {
            if(_isSettingsChanged != true)
            {
                _gameSettings.CancelSettings();
                _view.UpdateViewOptions(_gameSettings.Model);
            }

            _menuMonitor.CurrentState = MenuState.Start;
        }

        #endregion

        #region Change Settings Actions

        private void OnResolutionChange(int index)
        {
            var newResolution = _gameSettings.ResolutionList[index];
            _gameSettings.Model.ChangeResolution(newResolution.width, newResolution.height);
        }

        private void OnShadowChange(int index)
        {
            _gameSettings.Model.ChangeShadow(index);
        }

        private void OnFullScreenChange(bool value)
{
            _gameSettings.Model.ChangeFullScreenMode(value);
        }

        private void OnVSyncChange(bool value)
{
            _gameSettings.Model.ChangeVSyncMode(value);
        }

        private void OnMasterVolumeChange(float value)
        {
            _gameSettings.Model.ChangeMasterVolume(value);
        }

        private void OnMusicVolumeChange(float value)
        {
            _gameSettings.Model.ChangeMusicVolume(value);
        }

        private void OnVoiceVolumeChange(float value)
        {
            _gameSettings.Model.ChangeVoiceVolume(value);
        }

        private void OnEffectsVolumeChange(float value)
        {
            _gameSettings.Model.ChangeEffectsVolume(value);
        }

        #endregion
    }
}
