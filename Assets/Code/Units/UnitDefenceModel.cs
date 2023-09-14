using Units.Data;
using UnityEngine;

namespace Units
{
    public class UnitDefenceModel
    {
        private readonly IUnitDefenceData _data;

        public UnitDefenceModel(IUnitDefenceData data)
        {
            _data = data;
        }

        public int GetCutDefenceDamage(int damageAmount)
        {
            if(IsEvaded())
            {
                return 0;             
            }
            else
            {
                var result = damageAmount;

                return result;
            }
        }

        private bool IsEvaded()
        {
            var result = Random.Range(0, 101) >= _data.EvadeChance;
            return result;
        }

    }
}
