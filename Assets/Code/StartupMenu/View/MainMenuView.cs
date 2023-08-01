using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StartupMenu
{
    public class MainMenuView : MonoBehaviour
    {

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private AudioSource _clickAudioSource;

        internal void Init(
            UnityAction startGame, 
            UnityAction openSettings, 
            UnityAction exit,
            AudioSource clickAudioSource)
        {
            _clickAudioSource = clickAudioSource;

            _playButton.onClick.AddListener(startGame);
            _playButton.onClick.AddListener(PlayClickSound);

            _settingsButton.onClick.AddListener(openSettings);
            _settingsButton.onClick.AddListener(PlayClickSound);

            _quitButton.onClick.AddListener(exit);
            _quitButton.onClick.AddListener(PlayClickSound);
        }

        private void PlayClickSound() 
            => _clickAudioSource.Play();

        protected void OnDestroy()
        {
            _playButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }

    }
}

