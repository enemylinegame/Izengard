using System;
using UnityEngine;

namespace Units.Data
{
    [Serializable]
    public class DamageData : IUnitDamageData
    {
        [SerializeField] private float _baseDamage = 10f;
        [SerializeField] private float _fireDamage = 10f;
        [SerializeField] private float _coldDamage = 10f;

        #region IUnitDamageData

        float IUnitDamageData.BaseDamage => _baseDamage;

        float IUnitDamageData.FireDamage => _fireDamage;

        float IUnitDamageData.ColdDamage => _coldDamage;

        #endregion
    }
}
