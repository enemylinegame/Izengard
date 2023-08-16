using System;
using System.Collections.Generic;
using Audio_System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";

        private readonly GameSettingsManager _settingsManager;
        private readonly StateModel _menuMonitor;
        private readonly UIAudioProvider _uIAudioProvider;

        private Dictionary<SettingsMenuActionType, Action> _settingsMenuActions;
        private Dictionary<GameSettingsType, Action<object>> _changeSettingsActions;

        private SettingsMenuView _view;

        private bool _isSettingsChanged;

        public SettingsMenuController(
            Transform placeForUI, 
            GameSettingsManager settingsManager,
            ISettingsData baseSettings,
            StateModel menuMonitor,
            UIAudioProvider uIAudioProvider)
        {
            _settingsManager = settingsManager;
            _menuMonitor = menuMonitor;
            _uIAudioProvider = uIAudioProvider;

            _settingsMenuActions = GetMenuActions();
            _changeSettingsActions = GetSettingsAction();

            _view = LoadView(placeForUI);

            _view.Init(
                _settingsMenuActions, 
                _changeSettingsActions, 
                baseSettings, 
                _settingsManager.ResolutionList);

            _view.UpdateViewOptions(_settingsManager.GameSttingsModel);

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
            PlayClickSound();

            _settingsManager.SaveGameSettings();

            _isSettingsChanged = true;
        }

        private void RestoreToDefautls()
        {
            PlayClickSound();

            _settingsManager.RestoreGameSettings();
            _view.UpdateViewOptions(_settingsManager.GameSttingsModel);

            _isSettingsChanged = true;
        }

        private void BackToMenu()
        {
            PlayClickSound();

            if (_isSettingsChanged != true)
            {
                _settingsManager.LoadGameSettings();
                _view.UpdateViewOptions(_settingsManager.GameSttingsModel);
            }

            _menuMonitor.CurrentState = MenuState.Start;
        }

        private void PlayClickSound()
        {
            _uIAudioProvider.PlayButtonClickCound();
        }

        #endregion

        #region Change Settings Actions

        private void OnResolutionChange(int index)
        {
            var newResolution = _settingsManager.ResolutionList[index];
            _settingsManager.GameSttingsModel.ChangeResolution(newResolution.width, newResolution.height);
        }

        private void OnShadowChange(int index)
        {
            _settingsManager.GameSttingsModel.ChangeShadow(index);
        }

        private void OnFullScreenChange(bool value)
{
            _settingsManager.GameSttingsModel.ChangeFullScreenMode(value);
        }

        private void OnVSyncChange(bool value)
{
            _settingsManager.GameSttingsModel.ChangeVSyncMode(value);
        }

        private void OnMasterVolumeChange(float value)
        {
            _settingsManager.GameSttingsModel.ChangeMasterVolume(value);
        }

        private void OnMusicVolumeChange(float value)
        {
            _settingsManager.GameSttingsModel.ChangeMusicVolume(value);
        }

        private void OnVoiceVolumeChange(float value)
        {
            _settingsManager.GameSttingsModel.ChangeVoiceVolume(value);
        }

        private void OnEffectsVolumeChange(float value)
        {
            _settingsManager.GameSttingsModel.ChangeEffectsVolume(value);
        }

        #endregion
    }
}
