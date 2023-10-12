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

        private readonly IParametr<int> _armorPoints;

        private readonly IParametr<int> _baseShieldPoints;
        private readonly IParametr<int> _fireShieldPoints;
        private readonly IParametr<int> _coldShieldPoints;

        private readonly float _evadeChance;
        private readonly IUnitResistanceData _unitResistance;

        public IUnitDefenceData DefenceData => _defenceData;

        public IParametr<int> BaseShieldPoints => _baseShieldPoints;

        public IParametr<int> FireShieldPoints => _fireShieldPoints;

        public IParametr<int> ColdShieldPoints => _coldShieldPoints;

        public IParametr<int> ArmorPoints => _armorPoints;

        public UnitDefenceModel(IUnitDefenceData defenceData)
        {
            _defenceData = defenceData;

            _evadeChance = _defenceData.EvadeChance;

            _armorPoints = 
                new ParametrModel<int>(_defenceData.ArmorPoints, 0, int.MaxValue);
            _baseShieldPoints = 
                new ParametrModel<int>(_defenceData.ShieldData.BaseShieldPoints, 0, int.MaxValue);
            _fireShieldPoints =
                  new ParametrModel<int>(_defenceData.ShieldData.FireShieldPoints, 0, int.MaxValue);
            _coldShieldPoints =
                new ParametrModel<int>(_defenceData.ShieldData.ColdShieldPoints, 0, int.MaxValue);

            _unitResistance = _defenceData.ResistData;
        }

        public int GetAfterDefDamage(IDamage damageData)
        {
            var resultDamage = 0;

            if (IsEvaded() == false)
            {
                resultDamage = ApplyDefence(damageData);

                return resultDamage;             
            }

            return resultDamage;
        }

        private bool IsEvaded()
        {
            var result = Random.Range(0, 101) <= _evadeChance;
            return result;
        }

        private int ApplyDefence(IDamage damageData)
        {
            int resultDamage = 0;

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

        private bool CheckShield(IParametr<int> shield)
        {
            var shieldPoints = shield.GetValue();
            if(shieldPoints > 0)
            {
                shield.SetValue(--shieldPoints);
                return true;
            }
            return false;
        }

        private int UseResist(float innerDamage, float resistValue)
        {
            var resultDamage = 0;

            if (resistValue != 0)
            {
                resultDamage = (int)(innerDamage * (1 / resistValue));
            }
            else
            {
                resultDamage = (int)innerDamage;
            }

            return resultDamage;
        }

        private int UseArmor(int innerDamage)
        {
            int resultDamage = innerDamage;
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
