using System.Collections.Generic;
using ResourceSystem;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace Code.Units.HireDefendersSystem
{
    public class PaymentDefendersSystem
    {
        private readonly GlobalStock _globalStock;

        public PaymentDefendersSystem(GlobalStock globalStock)
        {
            _globalStock = globalStock;
        }


        public bool PayForDefender(List<ResourcePriceModel> hireCost)
        {
            if (hireCost == null) return false;

            bool isResourcesEnough = true;

            for (int i = 0; i < hireCost.Count; i++)
            {
                ResourcePriceModel pricePart = hireCost[i];
                if (!_globalStock.CheckResourceInStock(pricePart.ResourceType, pricePart.Cost))
                {
                    isResourcesEnough = false;
                    break;
                }
            }

            if (isResourcesEnough)
            {
                for (int i = 0; i < hireCost.Count; i++)
                {
                    ResourcePriceModel pricePart = hireCost[i];
                    _globalStock.GetResourceFromStock(pricePart.ResourceType, pricePart.Cost);
                }
            }
            else
            {
                Debug.Log("PaymentDefendersSystem->PayForDefender: Not enough recourses to pay for the defender.");
            }

            return isResourcesEnough;
        }

        public void ReturnCostForCancelHireDefender(List<ResourcePriceModel> hireCost)
        {
            if (hireCost == null) return;

            for (int i = 0; i < hireCost.Count; i++)
            {
                ResourcePriceModel pricePart = hireCost[i];
                _globalStock.AddResourceToStock(pricePart.ResourceType, pricePart.Cost);
            }
        }
        
        
    }
}