using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StartupMenu 
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _applySettingsButton;
        [SerializeField] private Button _restoreToDefaultsButton;
        [SerializeField] private Button _backToMenuButton;

        [Space(10)]
        [Header("Graphics Settings")]
        [Space(2)]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _shadowDropdown;
        [Space(5)]
        [SerializeField] private Toggle _fullScreenToggle;
        [SerializeField] private Toggle _vsyncToggle;

        [Space(10)]
        [Header("Sound Settings")]
        [Space(2)]
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _voiceVolumeSlider;
        [SerializeField] private Slider _effectsVolumeSlider;

        private AudioSource _clickAudioSource;
        private List<Resolution> _resolutions;
        
        public void Init(
            IDictionary<SettingsMenuActionType, Action> settingsMenuActions,
            IDictionary<GameSettingsType, Action<object>> actionsDictionary,
            ISettingsData baseData,
            IList<Resolution> resolutions,
            AudioSource clickAudioSource)
        {
            _clickAudioSource = clickAudioSource;

            Subscribe(settingsMenuActions, actionsDictionary);

            SetupVolumeSliders(baseData);

            CreateResolutions(resolutions);
        }

        private void Subscribe(
            IDictionary<SettingsMenuActionType, Action> settingsMenuActions, 
            IDictionary<GameSettingsType, Action<object>> actionsDictionary)
        {
            _applySettingsButton.onClick
                .AddListener(() => settingsMenuActions[SettingsMenuActionType.ApplySettings]?.Invoke());

            _applySettingsButton.onClick.AddListener(PlayClickSound);

            _restoreToDefaultsButton.onClick
                .AddListener(() => settingsMenuActions[SettingsMenuActionType.RestoreSettings]?.Invoke());

            _restoreToDefaultsButton.onClick.AddListener(PlayClickSound);

            _backToMenuButton.onClick
                .AddListener(() => settingsMenuActions[SettingsMenuActionType.BackToMenu]?.Invoke());

            _backToMenuButton.onClick.AddListener(PlayClickSound);

            SuscribeViewObjcetToAction(_resolutionDropdown, actionsDictionary[GameSettingsType.Resolution]);
            SuscribeViewObjcetToAction(_shadowDropdown, actionsDictionary[GameSettingsType.ShadowQuality]);
            SuscribeViewObjcetToAction(_fullScreenToggle, actionsDictionary[GameSettingsType.FullScreenMode]);
            SuscribeViewObjcetToAction(_vsyncToggle, actionsDictionary[GameSettingsType.VSyncMode]);
            SuscribeViewObjcetToAction(_masterVolumeSlider, actionsDictionary[GameSettingsType.MasterVolume]);
            SuscribeViewObjcetToAction(_musicVolumeSlider, actionsDictionary[GameSettingsType.MusicVolume]);
            SuscribeViewObjcetToAction(_voiceVolumeSlider, actionsDictionary[GameSettingsType.VoiceVolume]);
            SuscribeViewObjcetToAction(_effectsVolumeSlider, actionsDictionary[GameSettingsType.EffectsVolume]);
        }

        private void PlayClickSound() 
            => _clickAudioSource.Play();


        private void SuscribeViewObjcetToAction(MonoBehaviour signableObject, Action<object> action)
        {
            switch (signableObject)
            {
                case TMP_Dropdown dropdow:
                    {
                        dropdow.onValueChanged.AddListener(sender => action?.Invoke(sender));
                        break;
                    }
                case Toggle toggle: 
                    {
                        toggle.onValueChanged.AddListener(sender => action?.Invoke(sender));
                        break;
                    }
                case Slider slider:
                    {
                        slider.onValueChanged.AddListener(sender => action?.Invoke(sender));
                        break;
                    }
            }         
        }

        private void SetupVolumeSliders(ISettingsData data)
        {
            _masterVolumeSlider.maxValue = data.MixerMaxValue;
            _masterVolumeSlider.minValue = data.MixerMinValue;

            _musicVolumeSlider.maxValue = data.MixerMaxValue;
            _musicVolumeSlider.minValue = data.MixerMinValue;

            _voiceVolumeSlider.maxValue = data.MixerMaxValue;
            _voiceVolumeSlider.minValue = data.MixerMinValue;

            _effectsVolumeSlider.maxValue = data.MixerMaxValue;
            _effectsVolumeSlider.minValue = data.MixerMinValue;
        }

        private void CreateResolutions(IList<Resolution> resolutions)
        {
            _resolutionDropdown.ClearOptions();
            
            _resolutions = new List<Resolution>();

            var options = new List<string>();

            for (int i = 0; i < resolutions.Count; i++)
            {
                _resolutions.Add(resolutions[i]);
                string option = $"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRate}Hz";
                options.Add(option);
            }
            _resolutionDropdown.AddOptions(options);
        }

        public void UpdateViewOptions(ISettingsData data)
        {
            var currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Count; i++) 
            {
                if (_resolutions[i].width == data.ResolutionWidth
                    && _resolutions[i].height == data.ResolutionHeight)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            _shadowDropdown.value = data.ShadowId;
            _shadowDropdown.RefreshShownValue();

            _fullScreenToggle.isOn = data.IsFullScreenOn;
            _fullScreenToggle.onValueChanged?.Invoke(_fullScreenToggle.isOn);

            _vsyncToggle.isOn = data.IsVSyncOn;
            _vsyncToggle.onValueChanged?.Invoke(_vsyncToggle.isOn);      

            _masterVolumeSlider.value = data.MasterVolumeValue;
            _masterVolumeSlider.onValueChanged?.Invoke(_masterVolumeSlider.value);

            _musicVolumeSlider.value = data.MusicVolumeValue;
            _musicVolumeSlider.onValueChanged?.Invoke(_musicVolumeSlider.value);

            _voiceVolumeSlider.value = data.VoiceVolumeValue;
            _voiceVolumeSlider.onValueChanged?.Invoke(_voiceVolumeSlider.value);
            
            _effectsVolumeSlider.value = data.EffectsVolumeValue;
            _effectsVolumeSlider.onValueChanged?.Invoke(_effectsVolumeSlider.value);
        }

        protected void OnDestroy()
        {
            _applySettingsButton.onClick.RemoveAllListeners();
            _backToMenuButton.onClick.RemoveAllListeners();
            _restoreToDefaultsButton.onClick.RemoveAllListeners();

            _resolutionDropdown.onValueChanged.RemoveAllListeners();
            _shadowDropdown.onValueChanged.RemoveAllListeners();

            _fullScreenToggle.onValueChanged.RemoveAllListeners();
            _vsyncToggle.onValueChanged.RemoveAllListeners();

            _masterVolumeSlider.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider.onValueChanged.RemoveAllListeners();
            _voiceVolumeSlider.onValueChanged.RemoveAllListeners();
            _effectsVolumeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}


