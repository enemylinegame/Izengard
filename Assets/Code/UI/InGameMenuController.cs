﻿using System;
using Code.Game;
using Code.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    public class InGameMenuController : IDisposable
    {
        private readonly int _mainMenuSceneId = 0;
        private readonly int _gameSceneId = 1;

        private readonly InGameMenuUI _inGameMenuUI;
        private readonly PauseManager _pauseManager;
        private readonly KeyInputController _keyInputController;

        private bool _isPauseMode;

        public InGameMenuController(InGameMenuUI inGameMenuUI, PauseManager pauseManager,
            KeyInputController keyInputController)
        {
            _inGameMenuUI = inGameMenuUI;
            _pauseManager = pauseManager;
            _keyInputController = keyInputController;
            _keyInputController.OnCancelAxisClick += OnCancelButtonClick;
            _inGameMenuUI.ContinueButton.onClick.AddListener(OnContinueButtonClick);
            _inGameMenuUI.RestartButton.onClick.AddListener(OnRestartButtonClick);
            _inGameMenuUI.QuitButton.onClick.AddListener(OnQuitButtonClick);
            _inGameMenuUI.SettingsButton.onClick.AddListener(OnSettingsButtonClick);
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
            OffPauseMode();
        }

        private void OnRestartButtonClick()
        {
            OffPauseMode();
            SceneManager.LoadScene(_gameSceneId);
        }

        private void OnQuitButtonClick()
        {
            OffPauseMode();
            SceneManager.LoadScene(_mainMenuSceneId);
        }

        private void OnSettingsButtonClick()
        {
            Debug.Log("InGameMenuController->OnSettingsButtonClick:");
        }

        private void OnPauseMode()
        {
            if (!_isPauseMode)
            {
                _inGameMenuUI.RootGameObject.SetActive(true);
                _pauseManager.OnPause();
                _isPauseMode = true;
            }
        }

        private void OffPauseMode()
        {
            if (_isPauseMode)
            {
                _inGameMenuUI.RootGameObject.SetActive(false);
                _pauseManager.OffPause();
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