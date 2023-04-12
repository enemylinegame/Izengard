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
    private readonly Dictionary<TypeOfBuildings, IPoolController<GameObject>> _pools = 
        new Dictionary<TypeOfBuildings, IPoolController<GameObject>>();


    public BuildBuildings(GlobalBuildingsModels globalBuildingsModels, BuildGenerator buildGenerator)
    {
        _buildGenerator = buildGenerator;

        FillPools(globalBuildingsModels);
    }

    private void FillPools(GlobalBuildingsModels globalBuildingsModels)
    {
        var buildingsType = Enum.GetValues(typeof(TypeOfBuildings)).Cast<TypeOfBuildings>();
        foreach (var buildingType in buildingsType)
        {
            if (TryAddPool(buildingType, globalBuildingsModels.FindItemMarketBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindItemProduceBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindResurseMarketBuildingModel)) continue;
            if (TryAddPool(buildingType, globalBuildingsModels.FindResurseProduceBuildingModel)) continue;
        }
    }

    private bool TryAddPool(TypeOfBuildings typeOfBuildings, Func<TypeOfBuildings, BuildingModel> getBuilding)
    {
        var building = getBuilding(typeOfBuildings);
        if (building != null)
        {
            _pools.Add(typeOfBuildings, new GameObjectPoolController(START_POOL_CAPACITY, building.GotBuildPrefab));
            return true;
        }
        return false;
    }

    public void BuildBuilding(TypeOfBuildings typeOfBuildings)
    {
        _buildGenerator.StartPlacingBuild(_pools[typeOfBuildings]);
    }
}
