using System;
using Izengard.UnitSystem;
using Izengard.UnitSystem.Data;
using UnityEngine;

namespace Izengard.SpawnSystem
{
    [Serializable]
    public class SpawnUnitInfo 
    {
        [SerializeField] private int _inPoolCopacity = 5;
        [SerializeField] private UnitSettings _unitSettings;
        [SerializeField] private GameObject _unitPrefab;

        public int InPoolCopacity => _inPoolCopacity;
        public IUnitData UnitSettings => _unitSettings;
        public GameObject UnitPrefab => _unitPrefab;  
    }
}
