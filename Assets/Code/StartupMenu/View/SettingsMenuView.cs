using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StartupMenu 
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _applySettingsButton;
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
            UnityAction applySettings,
            UnityAction backToMenu,
            UnityAction<int> onResolutionChange,
            IList<Resolution> resolutions,
            SettingsModel model, 
            AudioSource clickAudioSource)
        {
            _clickAudioSource = clickAudioSource;

            _applySettingsButton.onClick.AddListener(applySettings);
            _applySettingsButton.onClick.AddListener(PlayClickSound);

            _backToMenuButton.onClick.AddListener(backToMenu);
            _backToMenuButton.onClick.AddListener(PlayClickSound);

            _resolutionDropdown.onValueChanged.AddListener(onResolutionChange);

            _shadowDropdown.onValueChanged.AddListener(model.ChangeShadow);

            _fullScreenToggle.onValueChanged.AddListener(model.ChangeFullScreenMode);
            _vsyncToggle.onValueChanged.AddListener(model.ChangeVSyncMode);

            _masterVolumeSlider.onValueChanged.AddListener(model.ChangeMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(model.ChangeMusicVolume);
            _voiceVolumeSlider.onValueChanged.AddListener(model.ChangeVoiceVolume);
            _effectsVolumeSlider.onValueChanged.AddListener(model.ChangeEffectsVolume);

            CreateResolutions(resolutions);
        }

        private void PlayClickSound() 
            => _clickAudioSource.Play();


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

        public void UpdateViewOptions(SettingsModel model)
        {
            var currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Count; i++) 
            {
                if (_resolutions[i].width == model.CurrentResolutionWidth
                    && _resolutions[i].height == model.CurrentResolutionHeight)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            _shadowDropdown.value = model.CurrentShadowId;
            _shadowDropdown.RefreshShownValue();

            _fullScreenToggle.isOn = model.IsFullScreenOn;
            _fullScreenToggle.onValueChanged?.Invoke(_fullScreenToggle.isOn);

            _vsyncToggle.isOn = model.IsFVSyncOn;
            _vsyncToggle.onValueChanged?.Invoke(_vsyncToggle.isOn);      

            _masterVolumeSlider.value = model.MasterVolumeValue;
            _masterVolumeSlider.onValueChanged?.Invoke(_masterVolumeSlider.value);

            _musicVolumeSlider.value = model.MusicVolumeValue;
            _musicVolumeSlider.onValueChanged?.Invoke(_musicVolumeSlider.value);

            _voiceVolumeSlider.value = model.VoiceVolumeValue;
            _voiceVolumeSlider.onValueChanged?.Invoke(_voiceVolumeSlider.value);
            
            _effectsVolumeSlider.value = model.EffectsVolumeValue;
            _effectsVolumeSlider.onValueChanged?.Invoke(_effectsVolumeSlider.value);
        }

        protected void OnDestroy()
        {
            _applySettingsButton.onClick.RemoveAllListeners();
            _backToMenuButton.onClick.RemoveAllListeners();
            
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


