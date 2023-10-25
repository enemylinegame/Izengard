﻿using System;
using UnityEngine;

namespace UnitSystem.Data
{
    [Serializable]
    public class ShieldData : IUnitShieldData
    {
        [SerializeField] 
        [Min(0.0f)]
        private float _baseShieldPoints = 10;
        
        [SerializeField]
        [Min(0.0f)]
        private float _fireShieldPoints = 10;
        
        [SerializeField]
        [Min(0.0f)]
        private float _coldShieldPoints = 10;
      
        #region IUnitShieldData

        public float BaseShieldPoints => _baseShieldPoints;

        public float FireShieldPoints => _fireShieldPoints;

        public float ColdShieldPoints => _coldShieldPoints;

        #endregion
    }
}
