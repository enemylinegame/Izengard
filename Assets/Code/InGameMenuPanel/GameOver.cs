using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameMenuPanel
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameObject battleUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject phaseInfo;
        [SerializeField] private GameObject inBattleInPeaceButton;
        [SerializeField] private TMP_Text _gameOverMessage;
        public Button gameOverButton;
        public string _gameOverMessageText = "GAME OVER";
        ButtonPhaseChanger buttonPhaseChanger;

        private void Awake()
        {

        }

        private void Start()
        {
            gameOverUI.SetActive(false);
        }

        public void gameOver() //Добавить падение башни и смерть дефендеров
        {
            _gameOverMessage.text = _gameOverMessageText;
            gameOverUI.SetActive(true);
            battleUI.SetActive(false);
            phaseInfo.SetActive(false);
            inBattleInPeaceButton.SetActive(false);

        }

        public void restart()
        {
            gameOverUI.SetActive(false);
            phaseInfo.SetActive(true);
            inBattleInPeaceButton.SetActive(true);
            buttonPhaseChanger.changePhase(); //Исправить обращение к методу из другого скрипта
        }
    }
}
