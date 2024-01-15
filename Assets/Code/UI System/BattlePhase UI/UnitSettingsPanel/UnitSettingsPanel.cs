using DG.Tweening;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitSettingsPanel : MonoBehaviour
    {
        [SerializeField]
        private UnitParametrs _parametrs;
        [SerializeField]
        protected Button openButton;
        [SerializeField]
        protected Button closeButton;

        [SerializeField]
        private TMP_InputField _spawnQuantityField;
        [SerializeField]
        private Button _plusButton;
        [SerializeField]
        private Button _minusButton;

        [SerializeField]
        private Button _spawnButton;

        public UnitParametrs Parametrs => _parametrs;

        public event Action<int> OnSpawn;

        protected void Awake()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            _spawnQuantityField.text = "1";
            _plusButton.onClick.AddListener(() => ChangeQuantityFieldValue(1));
            _minusButton.onClick.AddListener(() => ChangeQuantityFieldValue(-1));

            _spawnButton.onClick.AddListener(SpawnQuantityUnits);
        }

        private void OpenPanel()
        {
            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            transform.DOLocalMoveX(-10, 0.2f, true);
        }

        private void ClosePanel()
        {
            openButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);

            transform.DOLocalMoveX(400, 0.2f, true);
        }

        private void ChangeQuantityFieldValue(int amount)
        {
            var currentValue = int.Parse(_spawnQuantityField.text);
            
            currentValue += amount;
            
            if (currentValue < 1) currentValue = 1;

            _spawnQuantityField.text = currentValue.ToString();
        }

        private void SpawnQuantityUnits()
        {
            OnSpawn?.Invoke(int.Parse(_spawnQuantityField.text));
        }

        public void ResetPanel()
        {
            _parametrs.ResetData();
            _spawnQuantityField.text = "1";
        }

        private void OnDestroy()
        {
            openButton.onClick.RemoveListener(OpenPanel);
            closeButton.onClick.RemoveListener(ClosePanel);

            _plusButton.onClick.RemoveAllListeners();
            _minusButton.onClick.RemoveAllListeners();

            _spawnButton.onClick.RemoveAllListeners();
        }
    }
}
