using UnityEngine;
using TMPro;
using UnitSystem;
using System;

namespace UI
{
    public class UnitStatsPanel : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _statsContainer;
        [SerializeField]
        private TMP_Text _unitName;
        [SerializeField]
        private TMP_Text _unitHealth;
        [SerializeField]
        private TMP_Text _unitArmor;
        [SerializeField]
        private TMP_Text _unitBaseShield;
        [SerializeField]
        private TMP_Text _unitFireShield;
        [SerializeField]
        private TMP_Text _unitColdShield;

        private IUnit _currentUnit;

    
        public void SetUnit(IUnit unit)
        {
            Dispose();

             _currentUnit = unit;

            _unitName.text = unit.Name;

            SubscribeUnit(unit);

            SetValues(unit);

            _statsContainer.SetActive(true);
        }

        private void SetValues(IUnit unit)
        {
            ChangeHealth(unit.Stats.Health.GetValue());
            ChangeArmor(unit.Defence.ArmorPoints.GetValue());
            ChangeBaseShield(unit.Defence.BaseShieldPoints.GetValue());
            ChangeFireShield(unit.Defence.FireShieldPoints.GetValue());
            ChangeColdShield(unit.Defence.ColdShieldPoints.GetValue());
        }

        public void Dispose()
        {
            if (_currentUnit != null)
            {
                UnsubscribeUnit(_currentUnit);
            }
         
            ResetView();
        }

        private void ResetView()
        {
            _unitName.text = "Unit Stats";

            _unitHealth.text = "0";
            _unitArmor.text = "0";
            _unitBaseShield.text = "0";
            _unitFireShield.text = "0";
            _unitColdShield.text = "0";

            _statsContainer.SetActive(false);
        }

        private void SubscribeUnit(IUnit unit) 
        {
            unit.Stats.Health.OnValueChange += ChangeHealth;
            unit.Defence.ArmorPoints.OnValueChange += ChangeArmor;
            unit.Defence.BaseShieldPoints.OnValueChange += ChangeBaseShield;
            unit.Defence.FireShieldPoints.OnValueChange += ChangeFireShield;
            unit.Defence.ColdShieldPoints.OnValueChange += ChangeColdShield;
        }

        private void UnsubscribeUnit(IUnit unit)
        {
            unit.Stats.Health.OnValueChange -= ChangeHealth;
            unit.Defence.ArmorPoints.OnValueChange -= ChangeArmor;
            unit.Defence.BaseShieldPoints.OnValueChange -= ChangeBaseShield;
            unit.Defence.FireShieldPoints.OnValueChange -= ChangeFireShield;
            unit.Defence.ColdShieldPoints.OnValueChange -= ChangeColdShield;
        }


        private void ChangeHealth(int hpValue)
        { 
            _unitHealth.text = $"{hpValue}";
        }

        private void ChangeArmor(float armorValue)
        {
            _unitArmor.text = $"{armorValue}";
        }

        private void ChangeBaseShield(float shieldValue)
        {
            _unitBaseShield.text = $"{shieldValue}";
        }

        private void ChangeFireShield(float shieldValue)
        {
            _unitFireShield.text = $"{shieldValue}";
        }

        private void ChangeColdShield(float shieldValue)
        {
            _unitColdShield.text = $"{shieldValue}";
        }


        private void Awake()
        {
            ResetView();
        }
    }

}
