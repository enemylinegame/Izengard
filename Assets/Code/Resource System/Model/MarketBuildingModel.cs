using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{ 
    [System.Serializable]
    public abstract class MarketBuildingModel<T> : BuildingModel where T:ScriptableObject 
    {
        public List<T> GoodsInTheStore => _goodsInTheStore;
        public float MarketCostModification => _marketCostModification;
        public int BuyObjectCount => _buyObjectCount;
        public GoldCost CurrentBuyCost => _currentBuyCost;
        public System.Action<T> PurchaiseObjectAction;
        public List<Product<T>> MarketBasket => _productsInBasket;

        [SerializeField] protected List<T> _goodsInTheStore;
        [SerializeField] protected float _marketCostModification;
        [SerializeField] protected GoldCost _currentBuyCost;
        [SerializeField] protected List<Product<T>> _productsInBasket;
        protected int _buyObjectCount;






        public MarketBuildingModel(MarketBuildingModel<T> baseBuilding) : base(baseBuilding)
        {
            _goodsInTheStore = new List<T>(baseBuilding.GoodsInTheStore);
            _productsInBasket = new List<Product<T>>();
            _marketCostModification = baseBuilding.MarketCostModification;
        }

        public void BuyBasket(GlobalResourceStock stock)
        {
           if (stock.PriceGoldFromGlobalStock(CurrentBuyCost))
            {
                foreach (Product<T> product in _productsInBasket)
                {
                    stock.AddObjectInStock(product.ObjectProduct,product.ProduceValue);
                }
                _productsInBasket = new List<Product<T>>();
                _currentBuyCost = new GoldCost(_currentBuyCost, 0);
            }
        }
        public void ChangeActuialBuyProductValue(int value)
        {
            _buyObjectCount = value;
        }
        public abstract void AddProductInBasket(T obj);
        
        
        public void ChangeMarketCostModification(float value)
        {
            _marketCostModification = value;
        }
        public override void AwakeModel()
        {
            _productsInBasket = new List<Product<T>>();
        }
    }
}
