using System.Collections.Generic;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;

public interface IBuildingModel :IIconHolder,ISelectable,IHealthHolder,INameHolder
    {
       public List<ResourcePriceModel> BuildingCost { get; }
        public GameObject BuildingPrefab { get; }
        public TierNumber TierNumber { get; }

        public BuildingTypes BuildingType { get; }

        public float BuildingTime { get; }



    }

