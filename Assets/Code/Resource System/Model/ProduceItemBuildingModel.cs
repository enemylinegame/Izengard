using EquipmentSystem;
using ResourceSystem;

namespace BuildingSystem
{
    [System.Serializable]    
    public class ProduceItemBuildingModel : ProduceProductBuildingModel<ItemModel, ItemProduct,ItemСarrierHolder>
    {        

        public ProduceItemBuildingModel (ProduceItemBuildingModel basebuilding):base(basebuilding)
        {
            
        }

        public override void AddProductForProduce(ItemProduct product)
        {
            ItemProduct tempProduct = new ItemProduct(product);
            ProductsWaitPaid.Add(tempProduct);
        }

        public override void GetPaidForProducts(GlobalResourceStock stock)
        {
            if (ProductsWaitPaid!=null)
            { 
                foreach (ItemProduct product in ProductsWaitPaid)
                {
                    if (product.ProducePrice.PricePaidFlag)
                    { 
                        stock.GetResurseForProduceFromGlobalStock(product);
                    }
                }
            }
        }
    }
}
