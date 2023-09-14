using System;
using UnityEngine;

namespace Izengard.Units.Data
{
    [Serializable]
    public class ShieldData : IUnitShieldData
    {
        [SerializeField] private float _baseShieldPoints = 10f;
        [SerializeField] private float _fireShieldPoints = 10f;
        [SerializeField] private float _coldShieldPoints = 10f;
      
        #region IUnitShieldData

        public float BaseShieldPoints => _baseShieldPoints;

        public float FireShieldPoints => _fireShieldPoints;

        public float ColdShieldPoints => _coldShieldPoints;

        #endregion
    }
}
