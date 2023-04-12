using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{ 
    [System.Serializable]   
    public class HouseBuildingModel : BuildingModel,IProduceWorkers
    {
        public int CurrentWorkerValue => _currentWorkerValue;

       [SerializeField] private int _currentWorkerValue;

        public HouseBuildingModel(HouseBuildingModel baseBuilding):base(baseBuilding)
        {
            _currentWorkerValue = baseBuilding.CurrentWorkerValue;
        }

        public override void AwakeModel()
        {
            
        }
    }
}
