using Abstraction;
using Tools;
using UnityEngine;

namespace UnitSystem.Model
{
    public class UnitDefenceModel : IUnitDefence
    {
        #region Consts

        private const int ARMOR_REDUCE_COEF = 2;
        private const int ARMOR_LOSE_POINT = 1;

        #endregion

        private readonly IUnitDefenceData _defenceData;

        private readonly IParametr<float> _armorPoints;

        private readonly IParametr<float> _baseShieldPoints;
        private readonly IParametr<float> _fireShieldPoints;
        private readonly IParametr<float> _coldShieldPoints;

        private readonly float _evadeChance;
        private readonly IUnitResistanceData _unitResistance;

        public IUnitDefenceData DefenceData => _defenceData;

        public IParametr<float> BaseShieldPoints => _baseShieldPoints;

        public IParametr<float> FireShieldPoints => _fireShieldPoints;

        public IParametr<float> ColdShieldPoints => _coldShieldPoints;

        public IParametr<float> ArmorPoints => _armorPoints;

        public UnitDefenceModel(IUnitDefenceData defenceData)
        {
            _defenceData = defenceData;

            _evadeChance = _defenceData.EvadeChance;

            _armorPoints = 
                new ParametrModel<float>(_defenceData.ArmorPoints, 0, float.MaxValue);
            _baseShieldPoints = 
                new ParametrModel<float>(_defenceData.ShieldData.BaseShieldPoints, 0, float.MaxValue);
            _fireShieldPoints =
                  new ParametrModel<float>(_defenceData.ShieldData.FireShieldPoints, 0, float.MaxValue);
            _coldShieldPoints =
                new ParametrModel<float>(_defenceData.ShieldData.ColdShieldPoints, 0, float.MaxValue);

            _unitResistance = _defenceData.ResistData;
        }

        public float GetAfterDefDamage(IDamage damageData)
        {
            float resultDamage = 0;

            if (IsEvaded() == false)
            {
                resultDamage = ApplyDefence(damageData);

                return resultDamage;             
            }

            return resultDamage;
        }

        private bool IsEvaded()
        {
            var gainedChance = Random.value * 100f;
            return gainedChance < _evadeChance;
        }

        private float ApplyDefence(IDamage damageData)
        {
            float resultDamage = 0;

            if (CheckShield(_baseShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.BaseDamage, _unitResistance.BaseDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_fireShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.FireDamage, _unitResistance.FireDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_coldShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.ColdDamage, _unitResistance.ColdDamageResist);
                resultDamage += damageAfterResist;
            }

            resultDamage = UseArmor(resultDamage);

            return resultDamage;
        }

        private bool CheckShield(IParametr<float> shield)
        {
            var shieldPoints = shield.GetValue();
            if(shieldPoints > 0)
            {
                shield.SetValue(--shieldPoints);
                return true;
            }
            return false;
        }

        private float UseResist(float innerDamage, float resistValue)
        {
            float resultDamage = 0;

            if (resistValue != 0)
            {
                resultDamage = innerDamage * (1 / resistValue);
            }
            else
            {
                resultDamage = innerDamage;
            }

            return resultDamage;
        }

        private float UseArmor(float innerDamage)
        {
            float resultDamage = innerDamage;
            var armorPoints = _armorPoints.GetValue();
            if (armorPoints != 0)
            {
                resultDamage /= ARMOR_REDUCE_COEF;
                var armoLost = armorPoints - ARMOR_LOSE_POINT;
                _armorPoints.SetValue(armoLost);
            }

            return resultDamage;
        }
    }
}
