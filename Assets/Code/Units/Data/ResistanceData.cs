using System;
using UnityEngine;

namespace Units.Data
{
    [Serializable]
    public class ResistanceData : IUnitResistanceData
    {
        [Range(0, 100)]
        [SerializeField] private float _baseDamageResist = 25f;
        [Range(0, 100)]
        [SerializeField] private float _fireDamageResist = 25f;
        [Range(0, 100)]
        [SerializeField] private float _coldDamageResist = 25f;

        #region IUnitResistanceData

        float IUnitResistanceData.BaseDamageResist => _baseDamageResist;

        float IUnitResistanceData.FireDamageResist => _fireDamageResist;

        float IUnitResistanceData.ColdDamageResist => _coldDamageResist;

        #endregion
    }
}
