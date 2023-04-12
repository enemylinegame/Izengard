using ResurseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{ 
    [System.Serializable]    
    public class ResurseMarkeBuildingModel : MarketBuildingModel<ResurseCraft>
    {


        public override void AddProductInBasket(ResurseCraft obj)
        {
            ResurseProduct product = new ResurseProduct(obj, _buyObjectCount, 0);
            _productsInBasket.Add(product);
            float tempCost = (product.ObjectProduct.CostInGold.Cost + _marketCostModification)*product.ProduceValue;
            _currentBuyCost.ChangeCost(_currentBuyCost.Cost + tempCost);

        }
        public ResurseMarkeBuildingModel(ResurseMarkeBuildingModel baseBuilding) : base(baseBuilding)
        {

        }
    }
}

