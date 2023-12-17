using TMPro;
using UnitSystem;
using UnitSystem.Data;
using UnityEngine;

namespace UI 
{
    public class UnitParametrs : MonoBehaviour
    {
        [SerializeField] private UnitSettings _unitData;

        [Header("Main Stats")]
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


        private void Awake()
        {
            Init(_unitData);
        }

        private void Init(IUnitData data)
        {
            _healthField.text = data.StatsData.HealthPoints.ToString();
            _sizeField.text = data.StatsData.Size.ToString();
            _speedField.text = data.StatsData.Speed.ToString();
            _detectRangeField.text = data.StatsData.DetectionRange.ToString();

            _evadeChanceField.text = data.DefenceData.EvadeChance.ToString();
            _armorPointsField.text = data.DefenceData.ArmorPoints.ToString();
            _baseShieldField.text = data.DefenceData.ShieldData.BaseShieldPoints.ToString();
            _fireShieldField.text = data.DefenceData.ShieldData.FireShieldPoints.ToString();
            _coldShieldField.text = data.DefenceData.ShieldData.ColdShieldPoints.ToString();
            _baseDamageResistField.text = data.DefenceData.ResistData.BaseDamageResist.ToString();
            _fireDamageResistField.text = data.DefenceData.ResistData.FireDamageResist.ToString();
            _coldDamageResistField.text = data.DefenceData.ResistData.ColdDamageResist.ToString();


            _minRangeField.text = data.OffenceData.MinRange.ToString();
            _maxRangeField.text = data.OffenceData.MaxRange.ToString();
            _castingTimeField.text = data.OffenceData.CastingTime.ToString();
            _attackTimeField.text = data.OffenceData.AttackTime.ToString();
            _criticalChanceField.text = data.OffenceData.CriticalChance.ToString();
            _critScaleField.text = data.OffenceData.CritScale.ToString();

            _baseDamageField.text = data.OffenceData.DamageData.BaseDamage.ToString();
            _fireDamageField.text = data.OffenceData.DamageData.FireDamage.ToString();
            _coldDamageField.text = data.OffenceData.DamageData.ColdDamage.ToString();
        }
    }
}


