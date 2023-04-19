using EquipmentSystem;
using ResourceSystem;

namespace BuildingSystem
{ 
    [System.Serializable]    
    public class ItemMarketBuildingModel : MarketBuildingModel<ItemModel>
    {

        public ItemMarketBuildingModel(ItemMarketBuildingModel baseBuilding) : base(baseBuilding)
        {
            
      
        }
        public override void AddProductInBasket(ItemModel obj)
        {
            ItemProduct product = new ItemProduct(obj, _buyObjectCount, 0);
            _productsInBasket.Add(product);
            float tempCost = product.ObjectProduct.CostInGold.Cost + _marketCostModification;
            _currentBuyCost.ChangeCost(_currentBuyCost.Cost + tempCost);

        }
        
         
        
    }
}
