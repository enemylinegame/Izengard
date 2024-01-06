using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using UI;
using System;
using ResourceSystem;

namespace InGameMenuPanel
{
    public class ButtonPhaseChanger : MonoBehaviour
    {

        //�����
        [Header("�����")]
        [SerializeField] private GameObject battleUI;
        [SerializeField] private GameObject PeaceUI;

        //������
        [Header("������")]
        [SerializeField] private Camera _camera;

        //������� ������ � ������ ���� (x/z)
        [Header("������� ������ � ������ ����")]
        private int peacePhaseCameraPosiiotnX = 6;
        private int peacePhaseCameraPosiiotnZ = -6;
        
        //������� ������ � ������ ���� (x/z)
        [Header("������� ������ � ������ ����")]
        private int battlePhaseCameraPosiiotnX = -13;
        private int battlePhaseCameraPosiiotnZ = -6;

        //����������, ���������� �� ������ ������
        [Header("������ �� ��������� �����")]
        [SerializeField] private TMP_Text phaseBeginMessage;

        //����
        [Header("����")]
        [SerializeField] private TMP_Text currentPhaseText;
        public string battlePhase = "Current phase: Batlle";
        public string ecoPhase = "Current phase: Peace";
        private string _currentPhase;

        //��������� ��������� � ������ ������ ����
        [Header("��������� � ������ ����")]
        [SerializeField] private string messageBattlePhase = "The Battle phase has begun!";

        //��������� ��������� � ������ ������ ����
        [Header("��������� � ������ ����")]
        [SerializeField] private string messageEcoPhase = "The Peace phase has begun";

        //������
        [Header("������")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private int messageDurationTime = _messageDurationTime;
        private static int _sec = 6;
        private int sec = _sec;
        private static int _min = 0;
        private int min = _min;
        private int deltaSec = 1;    
        private static int _messageDurationTime = 4;  

        //������
        [Header("������")]
        public Button changePhaseButton;

        //����������� � �������
        [Header("����������� � �������")]
        [SerializeField] private InputField inputField;
        [SerializeField] private ResourceList resList;
        [SerializeField] private TMP_Text currentGoldText;
        [SerializeField] private int _maxGoldValue = 99999999;
        private GlobalStock globalStock;
        private int _currentGoldValue;
        private int _addGoldValue;
        
        

        private void Start()
        {     
            timerText.GetComponent<TMP_Text>();
            inputField.GetComponent<InputField>();
            globalStock = new GlobalStock(resList);
            peacePhaseBegin();
        }

        private void Update()
        {
            if (messageDurationTime == 0)
            {
                closeMessage();
            }

            //������� �������� �� ������ ���� � ������
            if (sec == 0 && min == 0)
            {
                sec = _sec;
                min = _min;
                peacePhaseBegin();
            }
        }

        public void inBattle()
        {
            if (_currentPhase == "Peace")
            {
                battlePhaseBegin();
                _currentPhase = "Battle";
            }
        }

        public void battlePhaseBegin()
        {
            StopAllCoroutines();
            _currentPhase = "Battle";
            phaseBeginMessage.enabled = true;
            phaseBeginMessage.text = messageBattlePhase;
            currentPhaseText.text = battlePhase;
            changePhaseButton.interactable = false;
            battleUI.SetActive(true);
            StartCoroutine(ITimer());
            StartCoroutine(messageDuration());
            _camera.transform.position = new Vector3(battlePhaseCameraPosiiotnX, _camera.transform.position.y, battlePhaseCameraPosiiotnZ);
            PeaceUI.SetActive(false);    


        }

        public void peacePhaseBegin()
        {
            StopAllCoroutines();
            _currentPhase = "Peace";
            phaseBeginMessage.enabled = true;
            phaseBeginMessage.text = messageEcoPhase;
            currentPhaseText.text = ecoPhase;
            changePhaseButton.interactable = true;
            battleUI.SetActive(false);
            StartCoroutine(messageDuration());
            _camera.transform.position = new Vector3(peacePhaseCameraPosiiotnX, _camera.transform.position.y, peacePhaseCameraPosiiotnZ);
            PeaceUI.SetActive(true);

            goldAdder();
            inputField.text = "";
        }

        private void closeMessage()
        {
            StopCoroutine(messageDuration());
            phaseBeginMessage.enabled = false;
        }

        private void goldAdder()
        {
            _currentGoldValue = int.Parse(currentGoldText.text);

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

            globalStock.AddResourceToStock(ResourceType.Gold, _currentGoldValue);
            currentGoldText.text = $"{globalStock.GetAvailableResourceAccount(ResourceType.Gold)}";
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
    }
}
