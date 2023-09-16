using Izengard.Abstraction.Interfaces;
using Izengard.Tools;
using Izengard.Units.Data;
using UnityEngine;

namespace Izengard.Units
{
    public class UnitDefenceModel : IUnitDefence
    {
        private readonly IUnitDefenceData _defenceData;

        private readonly IParametr<int> _baseShieldPoints;
        private readonly IParametr<int> _fireShieldPoints;
        private readonly IParametr<int> _coldShieldPoints;
        
        public IUnitDefenceData DefenceData => _defenceData;

        public IParametr<int> BaseShieldPoints => _baseShieldPoints;

        public IParametr<int> FireShieldPoints => _fireShieldPoints;

        public IParametr<int> ColdShieldPoints => _coldShieldPoints;

        public UnitDefenceModel(IUnitDefenceData defenceData)
        {
            _defenceData = defenceData;

            _baseShieldPoints = 
                new ParametrModel<int>(_defenceData.ShieldData.BaseShieldPoints, 0, int.MaxValue);
            _fireShieldPoints =
                  new ParametrModel<int>(_defenceData.ShieldData.FireShieldPoints, 0, int.MaxValue);
            _coldShieldPoints =
                new ParametrModel<int>(_defenceData.ShieldData.ColdShieldPoints, 0, int.MaxValue);
        }

        public int GetAfterDefDamage(UnitDamage damageData)
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
            var result = Random.Range(0, 101) >= _defenceData.EvadeChance;
            return result;
        }

        private int ApplyDefence(UnitDamage damageData)
        {
            int result = 0;

            if (CheckShield(_baseShieldPoints) == false)
            {
                result +=
                    (int)(damageData.BaseDamage * (1 / _defenceData.ResistData.BaseDamageResist));
            }

            if (CheckShield(_fireShieldPoints) == false)
            {
                result +=
                    (int)(damageData.FireDamage * (1 / _defenceData.ResistData.FireDamageResist));
            }

            if (CheckShield(_coldShieldPoints) == false)
            {
                result +=
                  (int)(damageData.ColdDamage * (1 / _defenceData.ResistData.ColdDamageResist));
            }

            return result;
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
    }
}
