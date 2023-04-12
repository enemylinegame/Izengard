using UnityEngine;

namespace BuildingSystem
{
    [System.Serializable]    
    public class MainBuildingModel : IProduceWorkers
    {
        public int CurrentWorkerValue => _currentWorkerValue;

        [SerializeField] private int _currentWorkerValue;
        public MainBuildingModel(MainBuildingModel baseBuilding)
        {
            _currentWorkerValue = baseBuilding.CurrentWorkerValue;
        }
    }
}
