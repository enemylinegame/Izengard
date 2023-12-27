using System;
using UnityEngine;

namespace Abstraction
{
    [Serializable]
    public class ResistanceData : IResistanceData
    {
        [Range(0, 100)]
        [SerializeField] private float _baseDamageResist = 25f;
        [Range(0, 100)]
        [SerializeField] private float _fireDamageResist = 25f;
        [Range(0, 100)]
        [SerializeField] private float _coldDamageResist = 25f;

        #region IUnitResistanceData

        float IResistanceData.BaseDamageResist => _baseDamageResist;

        float IResistanceData.FireDamageResist => _fireDamageResist;

        float IResistanceData.ColdDamageResist => _coldDamageResist;

        #endregion
    }
}
