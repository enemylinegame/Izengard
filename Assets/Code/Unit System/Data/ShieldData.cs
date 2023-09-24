using System;
using UnityEngine;

namespace Izengard.UnitSystem.Data
{
    [Serializable]
    public class ShieldData : IUnitShieldData
    {
        [SerializeField] private int _baseShieldPoints = 10;
        [SerializeField] private int _fireShieldPoints = 10;
        [SerializeField] private int _coldShieldPoints = 10;
      
        #region IUnitShieldData

        public int BaseShieldPoints => _baseShieldPoints;

        public int FireShieldPoints => _fireShieldPoints;

        public int ColdShieldPoints => _coldShieldPoints;

        #endregion
    }
}
