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

        public AudioMixer audioMixer;

        public TMP_Dropdown resolutionDropdown;

        Resolution[] resolutions;

        void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            //могут быть ошибки

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void Init(UnityAction backToMenu)
        {
            _backToMenuButton.onClick.AddListener(backToMenu);
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
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
        }
    }
}


