using BuildingSystem;
using ResurseSystem;
using System;
using UnityEngine;

public interface IBuildingModel :IIconHolder,ISelectable,IHealthHolder,INameHolder
{
    public ResurseCost ThisBuildingCost { get; }     
    public GameObject BasePrefab { get; }
    public GameObject GotBuildPrefab { get; }
    public float BuildingTime { get; }
    public float CurrentBuildTime { get; }

    
    

    public void StartBuilding(float value);
    public void GetResurseFromGlobalStock(GlobalResurseStock globalStock);
    public void GetGoldCostForBuilding(GlobalResurseStock globalStock);


}
