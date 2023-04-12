using ResurseSystem;
using System;
using UnityEngine;
using UnityEngine.UI;


 namespace BuildingSystem
{ 
    [System.Serializable]
    public class BuildingView : MonoBehaviour,IDisposable
    {
        public BuildingModel ThisBuildingModel => _thisBuildingModel;
               
        
        private BuildingModel _thisBuildingModel;
        [SerializeField]
        private GameObject BuildingVisual;
        [SerializeField]
        private GlobalBuildingsModels _globalBuildingsModels;
        [SerializeField]
        private TypeOfBuildings _thisBuildingType;
        public Action<BuildingModel> BuildingBorn;
        public Action<BuildingModel> BuildingDie;

        private void Awake()
        {
            
            AwakeBuildModel();
            BuildingBorn += _globalBuildingsModels.AddBuildingInNeedResurseForBuildingList;
            BuildingDie += _globalBuildingsModels.BuildingDestroy;
            BuildingBorn?.Invoke(ThisBuildingModel);
            
        }
        
        public void Dispose()
        {
           
            BuildingBorn -= _globalBuildingsModels.AddBuildingInNeedResurseForBuildingList;
           
            BuildingDie?.Invoke(_thisBuildingModel);
            BuildingDie -= _globalBuildingsModels.BuildingDestroy;
            
        }
        public BuildingModel GetBuildingModel()
        {
            return ThisBuildingModel;
        }
        public void ChangeBuildingVisual(BuildingModel bmodel)
        {
            BuildingVisual = bmodel.BasePrefab;
        }
        public void AwakeBuildModel()
        {
           _thisBuildingModel= _globalBuildingsModels.GetModelToBuilding(_thisBuildingType);
        }
    }
}


