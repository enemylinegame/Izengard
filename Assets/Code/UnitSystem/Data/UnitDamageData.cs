using System;
using UnityEngine;

namespace UnitSystem.Data
{
    [Serializable]
    public class UnitDamageData : IUnitDamageData
    {
        [SerializeField]
        [Min(0.0f)]
        private float _baseDamage = 10f;

        [SerializeField]
        [Min(0.0f)] 
        private float _fireDamage = 10f;
        
        [SerializeField]
        [Min(0.0f)] 
        private float _coldDamage = 10f;

        #region IUnitDamageData

        public float BaseDamage => _baseDamage;

        public float FireDamage => _fireDamage;

        public float ColdDamage => _coldDamage;

        #endregion
    }
}
