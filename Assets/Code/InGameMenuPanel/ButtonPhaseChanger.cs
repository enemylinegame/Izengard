using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using UI;
using System;
using ResourceSystem;
using ResourceSystem.SupportClases;

namespace InGameMenuPanel
{
    public class ButtonPhaseChanger : MonoBehaviour
    {
        
        //Юайки
        [Header("Юайки")]
        [SerializeField] private GameObject battleUI;
        [SerializeField] private GameObject PeaceUI;

        //Камера
        [Header("Камера")]
        [SerializeField] private Camera _camera;

        //Позиция камеры в мирной фазе (x/z)
        [Header("Позиция камеры в мирной фазе")]
        private int peacePhaseCameraPosiiotnX = 6;
        private int peacePhaseCameraPosiiotnZ = -6;
        
        //Позиция камеры в боевой фазе (x/z)
        [Header("Позиция камеры в боевой фазе")]
        private int battlePhaseCameraPosiiotnX = -13;
        private int battlePhaseCameraPosiiotnZ = -6;

        //Переменная, отвечающая за вывыод текста
        [Header("Ссылка на выводимый текст")]
        [SerializeField] private TMP_Text phaseBeginMessage;

        //Фазы
        [Header("Фазы")]
        [SerializeField] private TMP_Text currentPhaseText;
        public string battlePhase = "Current phase: Batlle";
        public string ecoPhase = "Current phase: Peace";
        private string _currentPhase;

        //Выводимое сообщение в начале боевой фазы
        [Header("Сообщение в боевой фазе")]
        [SerializeField] private string messageBattlePhase = "The Battle phase has begun!";

        //Выводимое сообщение в начале мирной фазы
        [Header("Сообщение в мирной фазе")]
        [SerializeField] private string messageEcoPhase = "The Peace phase has begun";

        //Таймер
        [Header("Таймер")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private int messageDurationTime = _messageDurationTime;
        private static int _sec = 6;
        private static int _min = 0;
        private int sec = _sec;
        private int min = _min;
        private int deltaSec = 1;    
        private static int _messageDurationTime = 4;

        //Кнопка
        [Header("Кнопка")]
        [SerializeField] private TMP_Text buttonText;
        public Button changePhaseButton;

        //Манипуляции с золотом
        [Header("Манипуляции с золотом")]
        [SerializeField] private InputField inputField;
        [SerializeField] private ResourceList resList;
        [SerializeField] private TMP_Text currentGoldText;
        [SerializeField] private int _maxGoldValue = 99999999;
        [SerializeField] private TMP_Text currentGoldValueText;
        private GlobalStock globalStock;
        private int _currentGoldValue;
        private int _addGoldValue;

        //GameOver
        [SerializeField] private TMP_Text gameOver;
        [SerializeField] private GameOverPanel _gameOverScreen;
        [SerializeField] private Button _gameOverButton;

        private void Awake()
        {
            currentGoldValueText.text = "0";
        }

        private void Start()
        {     
            globalStock = new GlobalStock(resList);
            PeacePhaseBegin();
            gameOver.GetComponent<TMP_Text>();

            _gameOverButton.onClick.AddListener(GameOverPressed);
            _gameOverScreen.OnRestartPressed += RestartPressed;
        }

        private void RestartPressed()
        {
            globalStock.GetResourceFromStock(ResourceType.Gold, _currentGoldValue);
            PeacePhaseBegin();
        }

        private void GameOverPressed()
        {
            _gameOverScreen.GameOver();
        }

        private void Update()
        {
            if (messageDurationTime == 0)
            {
                CloseMessage();
            }
        }

        public void ChangePhase()
        {
            if (_currentPhase == "Peace")
            {
                BattlePhaseBegin();
                _currentPhase = "Battle";
            }
            else if (_currentPhase == "Battle")
            {
                PeacePhaseBegin();
                _currentPhase = "Peace";
            }
        }

        public void BattlePhaseBegin()
        {
            StopAllCoroutines();
            _currentPhase = "Battle";
            buttonText.text = "In Peace";
            phaseBeginMessage.enabled = true;
            phaseBeginMessage.text = messageBattlePhase;
            currentPhaseText.text = battlePhase;
            battleUI.SetActive(true);
            StartCoroutine(messageDuration());
            _camera.transform.position = new Vector3(battlePhaseCameraPosiiotnX, _camera.transform.position.y, battlePhaseCameraPosiiotnZ);
            PeaceUI.SetActive(false);

        }

        public void PeacePhaseBegin()
        {
            StopAllCoroutines();
            _currentPhase = "Peace";
            buttonText.text = "In Battle";
            phaseBeginMessage.enabled = true;
            phaseBeginMessage.text = messageEcoPhase;
            currentPhaseText.text = ecoPhase;
            changePhaseButton.interactable = true;
            battleUI.SetActive(false);
            StartCoroutine(messageDuration());
            _camera.transform.position = new Vector3(peacePhaseCameraPosiiotnX, _camera.transform.position.y, peacePhaseCameraPosiiotnZ);
            PeaceUI.SetActive(true);

            GoldAdder();
            inputField.text = "0";

            gameOver.text = "";
        }

        private void CloseMessage()
        {
            StopCoroutine(messageDuration());
            phaseBeginMessage.enabled = false;
        }

        private void GoldAdder()
        {
            _currentGoldValue = int.Parse(currentGoldValueText.text);

            if (inputField.text != "")
            {
                _addGoldValue = int.Parse(inputField.text);
            }

            if (_addGoldValue > _maxGoldValue)
            {
                _addGoldValue = _maxGoldValue - _currentGoldValue;
            }

            _currentGoldValue += _addGoldValue;
           
            if (_currentGoldValue > _maxGoldValue)
            {
                _currentGoldValue = _maxGoldValue;
            }

            if (_currentGoldValue < 0)
            {
                _currentGoldValue = 0;
            }

            globalStock.AddResourceToStock(ResourceType.Gold, _addGoldValue);
            currentGoldValueText.text = $"{globalStock.GetAvailableResourceAccount(ResourceType.Gold)}";
            _addGoldValue = 0;
        }

        IEnumerator messageDuration()
        {
            messageDurationTime = _messageDurationTime;
            while (messageDurationTime != 0)
            {
                messageDurationTime -= deltaSec;
                yield return new WaitForSeconds(1);
            }
        }
        /*
        IEnumerator ITimer()
        {
            while (true)
            {
                if (sec == 0 && min != 0)
                {
                    min--;
                    sec = 60;
                }
                sec -= deltaSec;
                timerText.text = min.ToString("D2") + ":" + sec.ToString("D2");

                yield return new WaitForSeconds(1);
            }
        } 
        */
        
    }
}
