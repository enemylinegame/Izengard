using Code.Game;
using Code.Player;
using UnityEngine;

namespace Code.UI
{
    public class InGameMenuController
    {
        private readonly InGameMenuUI _inGameMenuUI;
        private readonly PauseManager _pauseManager;

        private bool _isPauseMode;

        public InGameMenuController(InGameMenuUI inGameMenuUI, PauseManager pauseManager, 
            KeyInputController keyInputController)
        {
            _inGameMenuUI = inGameMenuUI;
            _pauseManager = pauseManager;
            keyInputController.OnCancelAxisClick += OnCancelButtonClick;
            _inGameMenuUI.ContinueButton.onClick.AddListener(OnContinueButtonClick);
            _inGameMenuUI.RestartButton.onClick.AddListener(OnRestartButtonClick);
            _inGameMenuUI.QuitButton.onClick.AddListener(OnQuitButtonClick);
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
            
        }

        private void OnQuitButtonClick()
        {
            
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
        
    }
}