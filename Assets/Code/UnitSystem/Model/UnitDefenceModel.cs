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

        private readonly IUnitData _data;

        private readonly IParametr<float> _armorPoints;

        private readonly IParametr<float> _baseShieldPoints;
        private readonly IParametr<float> _fireShieldPoints;
        private readonly IParametr<float> _coldShieldPoints;

        private readonly float _evadeChance;

        public IUnitData Data => _data;

        public IParametr<float> BaseShieldPoints => _baseShieldPoints;

        public IParametr<float> FireShieldPoints => _fireShieldPoints;

        public IParametr<float> ColdShieldPoints => _coldShieldPoints;

        public IParametr<float> ArmorPoints => _armorPoints;

        public UnitDefenceModel(IUnitData data)
        {
            _data = data;

            _evadeChance = _data.EvadeChance;

            _armorPoints = 
                new ParametrModel<float>(_data.ArmorPoints, 0, float.MaxValue);
            _baseShieldPoints = 
                new ParametrModel<float>(_data.BaseShieldPoints, 0, float.MaxValue);
            _fireShieldPoints =
                  new ParametrModel<float>(_data.FireShieldPoints, 0, float.MaxValue);
            _coldShieldPoints =
                new ParametrModel<float>(_data.ColdShieldPoints, 0, float.MaxValue);
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
                var damageAfterResist = UseResist(damageData.BaseDamage, _data.BaseDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_fireShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.FireDamage, _data.FireDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_coldShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.ColdDamage, _data.ColdDamageResist);
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
