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
            SettingsMenuController controller,
            ISettingsData baseData,
            IList<Resolution> resolutions,
            AudioSource clickAudioSource)
        {
            _clickAudioSource = clickAudioSource;

            _applySettingsButton.onClick.AddListener(controller.ApplySettings);
            _applySettingsButton.onClick.AddListener(PlayClickSound);

            _backToMenuButton.onClick.AddListener(controller.BackToMenu);
            _backToMenuButton.onClick.AddListener(PlayClickSound);

            _resolutionDropdown.onValueChanged.AddListener(controller.OnResolutionChange);
            _shadowDropdown.onValueChanged.AddListener(controller.OnShadowChange);

            _fullScreenToggle.onValueChanged.AddListener(controller.OnFullScreenChange);
            _vsyncToggle.onValueChanged.AddListener(controller.OnVSyncChange);

            _masterVolumeSlider.onValueChanged.AddListener(controller.OnMasterVolumeChange);
            _musicVolumeSlider.onValueChanged.AddListener(controller.OnMusicVolumeChange);
            _voiceVolumeSlider.onValueChanged.AddListener(controller.OnVoiceVolumeChange);
            _effectsVolumeSlider.onValueChanged.AddListener(controller.OnEffectsVolumeChange);

            SetupVolumeSliders(baseData);

            CreateResolutions(resolutions);
        }

        private void PlayClickSound() 
            => _clickAudioSource.Play();


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


