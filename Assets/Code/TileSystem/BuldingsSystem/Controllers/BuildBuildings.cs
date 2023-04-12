using BuildingSystem;
using Controllers.Pool;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuildBuildings
{
    private const int START_POOL_CAPACITY = 5;

    private readonly BuildGenerator _buildGenerator;
    private readonly Dictionary<BuildingConfig, IPoolController<GameObject>> _pools = 
        new Dictionary<BuildingConfig, IPoolController<GameObject>>();


    public BuildBuildings(List<BuildingConfig> globalBuildingsModels, BuildGenerator buildGenerator)
    {
        _buildGenerator = buildGenerator;

        //FillPools(globalBuildingsModels);
    }

    private void FillPools(List<BuildingConfig> globalBuildingsModels)
    {
       /* var buildingsType = Enum.GetValues(typeof(BuildingTypes)).Cast<BuildingTypes>();
        foreach (var buildingType in buildingsType)
        {
            if (TryAddPool(buildingType, globalBuildingsModels.FindItemMarketBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindItemProduceBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindResurseMarketBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindResurseProduceBuildingModel)) continue;
        }*/
       // _pools.Clear();
       foreach (BuildingConfig buildingConfig in globalBuildingsModels)
       {
           TryAddPool(buildingConfig, buildingConfig);
       }
    }

    private void TryAddPool(BuildingConfig buildingConfig, IBuildingModel model)//, Func<BuildingTypes, BuildingModel> getBuilding)
    {
        /*var building = getBuilding(buildingConfig);
        if (building != null)
        {*/
        //if(_pools.ContainsKey(buildingTypes)) return;
            _pools.Add(buildingConfig, new GameObjectPoolController(START_POOL_CAPACITY, model.BuildingPrefab));
           // return true;
       /* }
        return false;*/
    }

    // public void BuildBuilding(BuildingConfig buildingConfig)
    // {
    //     _buildGenerator.StartPlacingBuild(_pools[buildingConfig]);
    // }
    public Building BuildBuilding1(BuildingConfig buildingConfig)
    {
        var buiding = _buildGenerator.StartBuildingHouses(buildingConfig);
        return buiding;
    }
}