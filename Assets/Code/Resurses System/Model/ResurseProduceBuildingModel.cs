using System;
using System.Collections;
using System.Collections.Generic;
using ResurseSystem;
using UnityEngine;

namespace BuildingSystem
{
    [System.Serializable]    
    public class ResurseProduceBuildingModel : ProduceProductBuildingModel<ResurseCraft, ResurseProduct,ResurseHolder>
    {

        public ResurseProduceBuildingModel(ResurseProduceBuildingModel basebuilding) : base(basebuilding)
        {

        }
        public override void AwakeModel()
        {
            
        }
        public override void AddProductForProduce(ResurseProduct product)
        {
            ResurseProduct tempProduct = new ResurseProduct(product);
            ProductsWaitPaid.Add(tempProduct);
        }

        public override void GetPaidForProducts(GlobalResurseStock stock)
        {
            if(ProductsWaitPaid!=null&& ProductsWaitPaid.Count>0)
            { 
                for  (int i=0;i< ProductsWaitPaid.Count;i++)
                {
                    if (!ProductsWaitPaid[i].ProducePrice.PricePaidFlag)
                    {
                        stock.GetResurseForProduceFromGlobalStock(ProductsWaitPaid[i]);
                    }
                    else
                    {
                        AddProductForStartProduce(ProductsWaitPaid[i]);
                    }
                }
            }
        }
    }

}