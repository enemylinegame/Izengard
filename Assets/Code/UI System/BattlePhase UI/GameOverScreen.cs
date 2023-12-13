using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        public event Action OnQuitButton;
        public event Action OnRestartButton;
        
        [SerializeField] private GameObject _screenRoot;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _restartButton;

        private void Awake()
        {
            _quitButton.onClick.AddListener(QuitButtonClick);
            _restartButton.onClick.AddListener(RestartButtonClick);
        }

        public void Show()
        {
            _screenRoot.SetActive(true);
        }

        public void Hide()
        {
            _screenRoot.SetActive(false);
        }

        private void QuitButtonClick()
        {
            OnQuitButton?.Invoke();
        }

        private void RestartButtonClick()
        {
            OnRestartButton?.Invoke();
        }
    }
}