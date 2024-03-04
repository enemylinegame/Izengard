using DG.Tweening;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Abstraction;
using UnitSystem.Enum;
using System.Collections.Generic;
using UnitSystem;

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
        [SerializeField]
        private Button _saveDataButton;
        [SerializeField]
        private Button _restoreDataButton;

        public UnitParametrs Parametrs => _parametrs;

        public event Action<int> OnSpawn;
        public event Action<IUnitData> OnSaveUnitData;
        public event Action<UnitType> OnRestoreUnitData;

        public void InitPanel()
        {
            Subscribe();

            openButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);

            _spawnQuantityField.text = "1";

            _parametrs.Init();

            Hide();
        }

        private void Subscribe()
        {
            openButton.onClick.AddListener(OpenPanel);
            closeButton.onClick.AddListener(ClosePanel);

            _plusButton.onClick.AddListener(() => ChangeQuantityFieldValue(1));
            _minusButton.onClick.AddListener(() => ChangeQuantityFieldValue(-1));
            
            _spawnButton.onClick.AddListener(SpawnQuantityUnits);
            _saveDataButton.onClick.AddListener(SaveCurrentUnitData);
            _restoreDataButton.onClick.AddListener(RestoreCurrentUnitData);
        }

        private void Unsubscribe()
        {
            openButton.onClick.RemoveListener(OpenPanel);
            closeButton.onClick.RemoveListener(ClosePanel);

            _plusButton.onClick.RemoveAllListeners();
            _minusButton.onClick.RemoveAllListeners();

            _spawnButton.onClick.RemoveListener(SpawnQuantityUnits);
            _saveDataButton.onClick.RemoveListener(SaveCurrentUnitData);
            _restoreDataButton.onClick.RemoveListener(RestoreCurrentUnitData);
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

        private void SaveCurrentUnitData()
        {
            var unitData = _parametrs.GetData();
            OnSaveUnitData?.Invoke(unitData);
        }

        private void RestoreCurrentUnitData()
        {
            var unitData = _parametrs.GetData();
            OnRestoreUnitData?.Invoke(unitData.Type);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetFaction(FactionType faction) 
            => _parametrs.SetFaction(faction);

        public void SetUnitTypes(IList<UnitType> unitTypes)
            => _parametrs.FillUnitTypeDropDown(unitTypes);

        public void ChangeData(IUnitData data) 
            => _parametrs.SetUnitData(data);

        public void ResetPanel()
        {
            _parametrs.ResetData();
            _spawnQuantityField.text = "1";
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}
