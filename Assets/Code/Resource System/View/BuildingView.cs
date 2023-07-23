using System;
using UnityEngine;


namespace BuildingSystem
{ 
    [System.Serializable]
    public class BuildingView : MonoBehaviour,IDisposable
    {
        public ScriptableObject BuildingConfig => _buildingConfig;

        [SerializeField]
        private GameObject _buildingVisual;
        [SerializeField]
        private ScriptableObject _buildingConfig;

        private IBuildingModel _buildingModel;
        public Action<IBuildingModel> BuildingBorn;
        public Action<IBuildingModel> BuildingDie;

        private void Awake()
        {
            //BuildingBorn += _globalBuildingsModels.AddBuildingInNeedResurseForBuildingList;
           // BuildingDie += _globalBuildingsModels.BuildingDestroy;
           _buildingModel = (IBuildingModel)_buildingConfig;
            BuildingBorn?.Invoke(_buildingModel);
            
        }
        
        public void Dispose()
        {
           
           // BuildingBorn -= _globalBuildingsModels.AddBuildingInNeedResurseForBuildingList;
           
            BuildingDie?.Invoke(_buildingModel);
     //       BuildingDie -= _globalBuildingsModels.BuildingDestroy;
            
        }
        public IBuildingModel GetBuildingModel()
        {
            return _buildingModel;
        }
        
    }
}


