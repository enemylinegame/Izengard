using System.Collections.Generic;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;

public interface IBuildingModel :IIconHolder,ISelectable,IHealthHolder,INameHolder
    {
        //public ResourceCost BuildingCost { get; }
        public List<ResourcePriceModel> BuildingCost { get; }
        public GameObject BuildingPrefab { get; }
        public TierNumber TierNumber { get; }

        public BuildingTypes BuildingType { get; }
        //public GameObject GotBuildPrefab { get; }
        public float BuildingTime { get; }
        //public float CurrentBuildTime { get; }

        //public List<ResourceSystem.SupportClases.ResourceCostSup> ResourceCosts { get; }

    
    

        /*   public void StartBuilding(float value);
    public void GetResourceFromGlobalStock(GlobalResorceStock globalStock);
    public void GetGoldCostForBuilding(GlobalResorceStock globalStock);*/


    }

