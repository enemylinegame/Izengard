using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace StartupMenu
{
    public class SettingsMenuController : BaseController
    {
        private readonly string _viewPath = "UI/StartupMenuUI/OptionsMenuUI";
       
        private readonly AudioMixer _audioMixer;
        private readonly StateModel _menuMonitor;

        private readonly SettingsMenuModel _model;

        private readonly List<Resolution> _resolutionList = new List<Resolution>();

        private SettingsMenuView _view;

        public SettingsMenuController(Transform placeForUI, AudioMixer audioMixer, StateModel menuMonitor, AudioSource clickSource)
        {
            _audioMixer = audioMixer;
            _menuMonitor = menuMonitor;

            _model = new SettingsMenuModel();
            _model.OnSettingsChanged += ChangeGraphicsSettings;
            _model.OnSettingsChanged += ChangeSoundSettings;

            _view = LoadView(placeForUI);
            _view.Init(BackToMenu, _model, clickSource);

            foreach(var resolution in Screen.resolutions)
            {
                _resolutionList.Add(resolution);
            }

            _view.SetUpResolution(_resolutionList);
        }


        private void ChangeGraphicsSettings(SettingsType type)
        {
            if (type != SettingsType.Graphics)
                return;

            var currentResolution = _resolutionList[_model.CurrentResolutionId];            
            Screen.SetResolution(currentResolution.width, currentResolution.height, _model.IsFullScreenOn);

            QualitySettings.SetQualityLevel(_model.CurrentGraphicsId);

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


        private SettingsMenuView LoadView(Transform placeForUI)
        {
            GameObject objectView = Object.Instantiate(LoadPrefab(_viewPath), placeForUI, false);
            AddGameObject(objectView);
            return objectView.GetComponent<SettingsMenuView>();
        }

        private void BackToMenu()
        {
            _menuMonitor.CurrentState = MenuState.Start;
        }
    }
}
