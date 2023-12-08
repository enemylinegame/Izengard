using Abstraction;
using Tools;
namespace BattleSystem.MainTower
{
    public class MainTowerDefenceModel
    {
        #region Consts

        private const int ARMOR_REDUCE_COEF = 2;
        private const int ARMOR_LOSE_POINT = 1;

        #endregion

        private readonly ToweDefenceData _defenceData;

        private readonly IParametr<float> _armorPoints;

        private readonly IParametr<float> _baseShieldPoints;
        private readonly IParametr<float> _fireShieldPoints;
        private readonly IParametr<float> _coldShieldPoints;

        private readonly IResistanceData _resistanceData;

        public ToweDefenceData DefenceData => _defenceData;

        public IParametr<float> ArmorPoints => _armorPoints;

        public IParametr<float> BaseShieldPoints => _baseShieldPoints;

        public IParametr<float> FireShieldPoints => _fireShieldPoints;

        public IParametr<float> ColdShieldPoints => _coldShieldPoints;

     
        public MainTowerDefenceModel(ToweDefenceData defenceData)
        {
            _defenceData = defenceData;

            _armorPoints =
               new ParametrModel<float>(_defenceData.ArmorPoints, 0, float.MaxValue);
            _baseShieldPoints =
                new ParametrModel<float>(_defenceData.ShieldData.BaseShieldPoints, 0, float.MaxValue);
            _fireShieldPoints =
                  new ParametrModel<float>(_defenceData.ShieldData.FireShieldPoints, 0, float.MaxValue);
            _coldShieldPoints =
                new ParametrModel<float>(_defenceData.ShieldData.ColdShieldPoints, 0, float.MaxValue);

            _resistanceData = _defenceData.ResistData;
        }

        public float GetAfterDefDamage(IDamage damageData)
        {
            float resultDamage = 0;

            resultDamage = ApplyDefence(damageData);

            return resultDamage;
        }

        private float ApplyDefence(IDamage damageData)
        {
            float resultDamage = 0;

            if (CheckShield(_baseShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.BaseDamage, _resistanceData.BaseDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_fireShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.FireDamage, _resistanceData.FireDamageResist);
                resultDamage += damageAfterResist;
            }

            if (CheckShield(_coldShieldPoints) == false)
            {
                var damageAfterResist = UseResist(damageData.ColdDamage, _resistanceData.ColdDamageResist);
                resultDamage += damageAfterResist;
            }

            resultDamage = UseArmor(resultDamage);

            return resultDamage;
        }

        private bool CheckShield(IParametr<float> shield)
        {
            var shieldPoints = shield.GetValue();
            if (shieldPoints > 0)
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
