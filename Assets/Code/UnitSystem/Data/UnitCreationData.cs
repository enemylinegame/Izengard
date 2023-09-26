using System;
using UnityEngine;

namespace Izengard.UnitSystem.Data
{
    [Serializable]
    public class UnitCreationData
    {
        [SerializeField] private int _inPoolCopacity = 2;
        [SerializeField] private UnitSettings _unitSettings;
        [SerializeField] private GameObject _unitPrefab;

        public int InPoolCopacity => _inPoolCopacity;
        public IUnitData UnitSettings => _unitSettings;
        public GameObject UnitPrefab => _unitPrefab;
    }
}
