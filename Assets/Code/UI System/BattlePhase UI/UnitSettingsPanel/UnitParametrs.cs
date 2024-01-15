using Abstraction;
using System;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnitSystem;
using UnitSystem.Data;
using UnitSystem.Enum;
using UnitSystem.Model;
using UnityEngine;

namespace UI
{
    public class UnitParametrs : MonoBehaviour
    {
        [SerializeField] 
        private UnitSettings _defaultUnitData;
        [SerializeField] 
        private UnitPriorityListUI _unitPriorityList;

        #region UnitData UI Settings

        [Header("Main Stats")]
        [SerializeField] 
        private TMP_Text _factionTypeText;
        [SerializeField] 
        private TMP_Dropdown _typeDropDown;
        [SerializeField] 
        private TMP_Dropdown _roleDropDown;
        [SerializeField]
        private TMP_InputField _healthField;
        [SerializeField]
        private TMP_InputField _sizeField;
        [SerializeField]
        private TMP_InputField _speedField;
        [SerializeField]
        private TMP_InputField _detectRangeField;

        [Space(10)]
        [Header("Defence Stats")]
        [SerializeField]
        private TMP_InputField _evadeChanceField;
        [SerializeField]
        private TMP_InputField _armorPointsField;
        [SerializeField]
        private TMP_InputField _baseShieldField;
        [SerializeField]
        private TMP_InputField _fireShieldField;
        [SerializeField]
        private TMP_InputField _coldShieldField;
        [SerializeField]
        private TMP_InputField _baseDamageResistField;
        [SerializeField]
        private TMP_InputField _fireDamageResistField;
        [SerializeField]
        private TMP_InputField _coldDamageResistField;

        [Space(10)]
        [Header("Offence Stats")]
        [SerializeField]
        private TMP_Dropdown _attackTypeDropDown;
        [SerializeField]
        private TMP_Dropdown _abilityTypeDropDown;
        [SerializeField]
        private TMP_InputField _minRangeField;
        [SerializeField]
        private TMP_InputField _maxRangeField;
        [SerializeField]
        private TMP_InputField _castingTimeField;
        [SerializeField]
        private TMP_InputField _attackTimeField;
        [SerializeField]
        private TMP_InputField _criticalChanceField;
        [SerializeField]
        private TMP_InputField _critScaleField;
        [SerializeField]
        private TMP_InputField _baseDamageField;
        [SerializeField]
        private TMP_InputField _fireDamageField;
        [SerializeField]
        private TMP_InputField _coldDamageField;

        #endregion

        private FactionType _faction;

        public Action<UnitType> OnUnitTypeChange;

        public void Init()
        {
            if (_defaultUnitData == null)
                return;

            _faction = FactionType.None;

            _typeDropDown.onValueChanged.AddListener(UnitTypeChanged);

            SetUnitData(_defaultUnitData);
        }

        private void UnitTypeChanged(int value)
        {
            var unitType = (UnitType)value;
            OnUnitTypeChange?.Invoke(unitType);
        }

        public void SetUnitData(IUnitData data)
        {
            _factionTypeText.text = _faction.GetDescription();

            InitRoleDropDown(data);

            _unitPriorityList.Init(data.UnitPriorities);

            _healthField.text = data.HealthPoints.ToString();
            _sizeField.text = data.Size.ToString();
            _speedField.text = data.Speed.ToString();
            _detectRangeField.text = data.DetectionRange.ToString();

            _evadeChanceField.text = data.EvadeChance.ToString();
            _armorPointsField.text = data.ArmorPoints.ToString();
            _baseShieldField.text = data.BaseShieldPoints.ToString();
            _fireShieldField.text = data.FireShieldPoints.ToString();
            _coldShieldField.text = data.ColdShieldPoints.ToString();
            _baseDamageResistField.text = data.BaseDamageResist.ToString();
            _fireDamageResistField.text = data.FireDamageResist.ToString();
            _coldDamageResistField.text = data.ColdDamageResist.ToString();

            InitAttackTypeDropDown(data);
            InitAbilityTypeDropDown(data);

            _minRangeField.text = data.MinRange.ToString();
            _maxRangeField.text = data.MaxRange.ToString();
            _castingTimeField.text = data.CastingTime.ToString();
            _attackTimeField.text = data.AttackTime.ToString();
            _criticalChanceField.text = data.CriticalChance.ToString();
            _critScaleField.text = data.CritScale.ToString();

            _baseDamageField.text = data.BaseDamage.ToString();
            _fireDamageField.text = data.FireDamage.ToString();
            _coldDamageField.text = data.ColdDamage.ToString();
        }

        private void InitRoleDropDown(IUnitData data)
        {
            _roleDropDown.ClearOptions();

            var optData
                = new List<string> {
                        nameof(UnitRoleType.None),
                        nameof(UnitRoleType.Fodder),
                        nameof(UnitRoleType.Fighter),
                        nameof(UnitRoleType.Gunner),
                        nameof(UnitRoleType.Caster),
                        nameof(UnitRoleType.Tank),
                };

            _roleDropDown.AddOptions(optData);

            _roleDropDown.value = (int)data.Role;
        }

        private void InitAttackTypeDropDown(IUnitData data)
        {
            _attackTypeDropDown.ClearOptions();

            var optData
                = new List<string> {
                        nameof(UnitAttackType.None),
                        nameof(UnitAttackType.Melee),
                        nameof(UnitAttackType.Range)
                };

            _attackTypeDropDown.AddOptions(optData);

            _attackTypeDropDown.value = (int)data.AttackType;
        }

        private void InitAbilityTypeDropDown(IUnitData data)
        {
            _abilityTypeDropDown.ClearOptions();

            var optData
                = new List<string> {
                        nameof(UnitAbilityType.None),
                        nameof(UnitAbilityType.SelfShield),
                        nameof(UnitAbilityType.AllyShield),
                        nameof(UnitAbilityType.ShortRangedAttack),
                        nameof(UnitAbilityType.RangedAOE),
                        nameof(UnitAbilityType.ClosedAOE),
                        nameof(UnitAbilityType.LinedAOE),
                        nameof(UnitAbilityType.SpearDash),
                };

            _abilityTypeDropDown.AddOptions(optData);

            _abilityTypeDropDown.value = (int)data.AbilityType;
        }

        public void SetFaction(FactionType faction)
        {
            _faction = faction;
            _factionTypeText.text = _faction.GetDescription();
        }

        public void FillUnitTypeDropDown(IList<UnitType> unitTypes)
        {
            _typeDropDown.ClearOptions();
            var optData = new List<string>();
            for (int i = 0; i < unitTypes.Count; i++)
            {
                optData.Add(unitTypes[i].GetDescription());
            }

            _typeDropDown.AddOptions(optData);

            _typeDropDown.value = 0;
        }

        public IUnitData GetData()
        {
            var unitData = new UnitDataModel
            {
                Faction = _faction,
                Type = (UnitType)_typeDropDown.value,
                Role = (UnitRoleType)_roleDropDown.value,

                HealthPoints = int.Parse(_healthField.text),
                Size = float.Parse(_sizeField.text),
                Speed = float.Parse(_speedField.text),
                DetectionRange = float.Parse(_detectRangeField.text),

                UnitPriorities = _unitPriorityList.GetPriorityData(),

                EvadeChance = float.Parse(_evadeChanceField.text),
                ArmorPoints = float.Parse(_armorPointsField.text),
                BaseShieldPoints = float.Parse(_baseShieldField.text),
                FireShieldPoints = float.Parse(_fireShieldField.text),
                ColdShieldPoints = float.Parse(_coldShieldField.text),
                BaseDamageResist = float.Parse(_baseDamageResistField.text),
                FireDamageResist = float.Parse(_fireDamageResistField.text),
                ColdDamageResist = float.Parse(_coldDamageResistField.text),

                AttackType = (UnitAttackType)_attackTypeDropDown.value,
                AbilityType = (UnitAbilityType)_abilityTypeDropDown.value,

                MinRange = float.Parse(_minRangeField.text),
                MaxRange = float.Parse(_maxRangeField.text),
                CastingTime = float.Parse(_castingTimeField.text),
                AttackTime = float.Parse(_attackTimeField.text),
                CriticalChance = float.Parse(_criticalChanceField.text),
                CritScale = float.Parse(_critScaleField.text),
                FailChance = _defaultUnitData.FailChance,
                OnFailDamage = _defaultUnitData.OnFailDamage,
                BaseDamage = float.Parse(_baseDamageField.text),
                FireDamage = float.Parse(_fireDamageField.text),
                ColdDamage = float.Parse(_coldDamageField.text),
            };

            return unitData;
        }

        public void ResetData()
        {
            _unitPriorityList.ResetData();
            
            _faction = FactionType.None;

            SetUnitData(_defaultUnitData);
        }  
    }
}


