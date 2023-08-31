using System;
using Code.Game;
using Code.Player;
using UnityEngine;


namespace Code.UI
{
    public class InGameMenuController : IDisposable
    {
        private readonly InGameMenuPanelController _inGameMenuPanel;
        private readonly GameStateManager _gameStateManager;
        private readonly KeyInputController _keyInputController;
        private readonly SoundPlayer _soundPlayer;

        private bool _isPauseMode;

        public InGameMenuController(InGameMenuPanelController inGameMenuPanel,  
            GameStateManager gameStateManager, KeyInputController keyInputController, SoundPlayer soundPlayer)
        {
            _inGameMenuPanel = inGameMenuPanel;
            _gameStateManager = gameStateManager;
            _keyInputController = keyInputController;
            _keyInputController.OnCancelAxisClick += OnCancelButtonClick;
            _inGameMenuPanel.ContinueButton += OnContinueButtonClick;
            _inGameMenuPanel.RestartButton += OnRestartButtonClick;
            _inGameMenuPanel.QuitButton += OnQuitButtonClick;
            _inGameMenuPanel.SettingsButton += OnSettingsButtonClick;
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
                _inGameMenuPanel.ActivateMenu();
                _gameStateManager.OnPause();
                _isPauseMode = true;
            }
        }

        private void OffPauseMode()
        {
            if (_isPauseMode)
            {
                _inGameMenuPanel.DeactivateMenu();
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
            _inGameMenuPanel.ContinueButton -= OnContinueButtonClick;
            _inGameMenuPanel.RestartButton -= OnRestartButtonClick;
            _inGameMenuPanel.QuitButton -= OnQuitButtonClick;
            _inGameMenuPanel.SettingsButton -= OnSettingsButtonClick;
            _inGameMenuPanel.DisposeButtons();
        }

        #endregion

    }
}