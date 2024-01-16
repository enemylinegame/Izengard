using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameMenuPanel
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private GameObject battleUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject phaseInfo;
        [SerializeField] private GameObject inBattleInPeaceButton;
        [SerializeField] private TMP_Text _gameOverMessage;
        [SerializeField] private Button _restartButton;
        [SerializeField] private ButtonPhaseChanger _phaseChanger;
        public Button gameOverButton;
        public string _gameOverMessageText = "GAME OVER";

        public event Action OnRestartPressed;

        private void Awake()
        {

        }

        private void Start()
        {
            gameOverUI.SetActive(false);
            _restartButton.onClick.AddListener(RestartPressed);
        }

        private void RestartPressed()
        {
            Restart();
        }

        public void GameOver() //�������� ������� ����� � ������ ����������
        {
            _gameOverMessage.text = _gameOverMessageText;
            gameOverUI.SetActive(true);
            battleUI.SetActive(false);
            phaseInfo.SetActive(false);
            inBattleInPeaceButton.SetActive(false);

        }

        public void Restart()
        {
            gameOverUI.SetActive(false);
            phaseInfo.SetActive(true);
            inBattleInPeaceButton.SetActive(true);
            _phaseChanger.PeacePhaseBegin();

            OnRestartPressed?.Invoke();
        }
    }
}
