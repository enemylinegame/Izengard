using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StartupMenu 
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _backToMenuButton; 
        [SerializeField] private AudioMixer _audioMixer;
        
        [Space(10)]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _graphicsDropdown;
        [SerializeField] private TMP_Dropdown _shadowDropdown;

        [Space(10)]
        [SerializeField] private Toggle _fullScreenToggle;
        [SerializeField] private Toggle _vsyncToggle;
        [SerializeField] private Toggle _blurToggle;

        private Resolution[] resolutions;

        public void Init(
            UnityAction backToMenu, SettingsMenuModel model)
        {
            _backToMenuButton.onClick.AddListener(backToMenu);
            
            _resolutionDropdown.onValueChanged.AddListener(model.ChangeResolution);
            _graphicsDropdown.onValueChanged.AddListener(model.ChangeGraphics);
            _shadowDropdown.onValueChanged.AddListener(model.ChangeShadow);

            _fullScreenToggle.onValueChanged.AddListener(model.ChangeFullScreenMode);
            _vsyncToggle.onValueChanged.AddListener(model.ChangeVSyncMode);
            _blurToggle.onValueChanged.AddListener(model.ChangeBlurMode);

            resolutions = Screen.resolutions;

            SetUpResolution(resolutions);
        }

        private void SetUpResolution(IList<Resolution> resolutions)
        {
            _resolutionDropdown.ClearOptions();

            var options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Count; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }


        public void SetVolume(float volume)
        {
            _audioMixer.SetFloat("volume", volume);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        protected void OnDestroy()
        {
            _backToMenuButton.onClick.RemoveAllListeners();
            
            _resolutionDropdown.onValueChanged.RemoveAllListeners();
            _graphicsDropdown.onValueChanged.RemoveAllListeners();
            _shadowDropdown.onValueChanged.RemoveAllListeners();

            _fullScreenToggle.onValueChanged.RemoveAllListeners();
            _vsyncToggle.onValueChanged.RemoveAllListeners();
            _blurToggle.onValueChanged.RemoveAllListeners();
        }
    }
}


