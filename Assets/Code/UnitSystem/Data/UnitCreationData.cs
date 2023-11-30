using System;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [Serializable]
    public class UnitCreationData
    {
        [SerializeField] private int _poolCopacity = 2;
        [SerializeField] private UnitType _type;
        [SerializeField] private UnitSettings _unitSettings;
        [SerializeField] private GameObject _unitPrefab;

        public int PoolCopacity => _poolCopacity;
        public UnitType Type => _type;
        public IUnitData UnitSettings => _unitSettings;
        public GameObject UnitPrefab => _unitPrefab;

      
    }
}
