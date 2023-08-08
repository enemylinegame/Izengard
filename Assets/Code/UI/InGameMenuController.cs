using System;
using Code.Game;
using Code.Player;
using UnityEngine;


namespace Code.UI
{
    public class InGameMenuController : IDisposable
    {
        private readonly InGameMenuUI _inGameMenuUI;
        private readonly GameStateManager _gameStateManager;
        private readonly KeyInputController _keyInputController;
        private readonly SoundPlayer _soundPlayer;

        private bool _isPauseMode;

        public InGameMenuController(InGameMenuUI inGameMenuUI,  
            GameStateManager gameStateManager, KeyInputController keyInputController, SoundPlayer soundPlayer)
        {
            _inGameMenuUI = inGameMenuUI;
            _gameStateManager = gameStateManager;
            _keyInputController = keyInputController;
            _keyInputController.OnCancelAxisClick += OnCancelButtonClick;
            _inGameMenuUI.ContinueButton.onClick.AddListener(OnContinueButtonClick);
            _inGameMenuUI.RestartButton.onClick.AddListener(OnRestartButtonClick);
            _inGameMenuUI.QuitButton.onClick.AddListener(OnQuitButtonClick);
            _inGameMenuUI.SettingsButton.onClick.AddListener(OnSettingsButtonClick);
            _soundPlayer = soundPlayer;
        }

        private void OnCancelButtonClick()
        {
            if (_isPauseMode)
            {
                OffPauseMode();
            }
            else
            {
                OnPauseMode();
            }
        }

        private void OnContinueButtonClick()
        {
            _soundPlayer.PlayClickSound();
            OffPauseMode();
        }

        private void OnRestartButtonClick()
        {
            _soundPlayer.PlayClickSound();
            OffPauseMode();
            _gameStateManager.RestartGame();
        }

        private void OnQuitButtonClick()
        {
            _soundPlayer.PlayClickSound();
            OffPauseMode();
            _gameStateManager.SwitchToMainMenu();
        }

        private void OnSettingsButtonClick()
        {
            _soundPlayer.PlayClickSound();
            Debug.Log("InGameMenuController->OnSettingsButtonClick:");
        }

        private void OnPauseMode()
        {
            if (!_isPauseMode)
            {
                _inGameMenuUI.RootGameObject.SetActive(true);
                _gameStateManager.OnPause();
                _isPauseMode = true;
            }
        }

        private void OffPauseMode()
        {
            if (_isPauseMode)
            {
                _inGameMenuUI.RootGameObject.SetActive(false);
                _gameStateManager.OffPause();
                _isPauseMode = false;
            }
        }

        private void SettingsClosed()
        {
            
        }

        #region IDisposable

        public void Dispose()
        {
            _keyInputController.OnCancelAxisClick -= OnCancelButtonClick;
            _inGameMenuUI.ContinueButton.onClick.RemoveListener(OnContinueButtonClick);
            _inGameMenuUI.RestartButton.onClick.RemoveListener(OnRestartButtonClick);
            _inGameMenuUI.QuitButton.onClick.RemoveListener(OnQuitButtonClick);
            _inGameMenuUI.SettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        }

        #endregion

    }
}