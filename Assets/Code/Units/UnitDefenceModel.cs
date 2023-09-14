using Izengard.Damage;
using Izengard.Units.Data;
using UnityEngine;

namespace Izengard.Units
{
    public class UnitDefenceModel
    {
        private readonly IUnitDefenceData _data;

        private int _baseShieldPoints;
        private int _fireShieldPoints;
        private int _coldShieldPoints;

        public UnitDefenceModel(IUnitDefenceData data)
        {
            _data = data;
        }

        public int GetAfterDefDamage(IUnitDamage damageData)
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
            var result = Random.Range(0, 101) >= _data.EvadeChance;
            return result;
        }

        private int ApplyDefence(IUnitDamage damageData)
        {
            int result = 0;

            if(_baseShieldPoints > 0)
            { 
                _baseShieldPoints--;
            }
            else
            {
                result += 
                    (int)(damageData.BaseDamage * (1 / _data.ResistData.BaseDamageResist));
            }

            if (_fireShieldPoints > 0)
            {
                _fireShieldPoints--;
            }
            else
            {
                result += 
                    (int)(damageData.FireDamage * (1 / _data.ResistData.FireDamageResist));
            }

            if (_coldShieldPoints > 0)
            {
                _coldShieldPoints--;
            }
            else
            {
                result += 
                    (int)(damageData.ColdDamage * (1 / _data.ResistData.ColdDamageResist));
            }

            return result;
        }
    }
}
