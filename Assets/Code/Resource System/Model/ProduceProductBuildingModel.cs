using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{ 
    [System.Serializable]
    public abstract class ProduceProductBuildingModel<T,M,N> : BuildingModel where T:ScriptableObject where M:Product<T> where N:Holder<T>
    {
        public List<M> ProduceProduct => _produceProduct;
        public bool AutoProduceFlag => _autoProduceFlag;
        public Stock<T,N> ProducedObjectStock => _producedObjectStock;

        [SerializeField] protected List<M> _produceProduct;
        [SerializeField] protected bool _autoProduceFlag;
        [SerializeField] protected Stock<T,N> _producedObjectStock;
        [SerializeField] protected GlobalResourceStock globalResorceStock;
        protected List<M> ProductsWaitPaid;
        protected List<M> ProduceWaitProducts;

        public ProduceProductBuildingModel(ProduceProductBuildingModel<T,M,N> baseBuilding) :base(baseBuilding)
        {
            _produceProduct = new List<M>(baseBuilding.ProduceProduct);
            _autoProduceFlag = baseBuilding.AutoProduceFlag;
            _producedObjectStock = baseBuilding._producedObjectStock;
            ProductsWaitPaid = new List<M>();
            ProduceWaitProducts = new List<M>();
        }
        public override void AwakeModel()
        {
            
        }
        public void SetAutoproduceFlag(bool value)
        {
            _autoProduceFlag = value;
        }
        public void ChangeAutoproduceFlag()
        {
            _autoProduceFlag = !_autoProduceFlag;
        }
        public abstract void AddProductForProduce(M product);
        public void AddProductForStartProduce(M product)
        {
            if (product.ProducePrice.PricePaidFlag)
            {
                ProduceWaitProducts.Add(product);
                ProductsWaitPaid.Remove(product);
                
            }
        }
        
        public void ProductProduced(Product<T> product)
        {
            if (!AutoProduceFlag)
            { 
                ProduceWaitProducts.Remove((M)product);
            }
            else
            {
                ProduceWaitProducts.Remove((M)product);
                ProductsWaitPaid.Add((M)product);
            }
            globalResorceStock.AddObjectInStock(product.ObjectProduct,product.ProduceValue);
            product.ResetCostInResurse();
            
        }
        public void StartProduce(float time)
        {            
            if (ProduceWaitProducts!=null && ProduceWaitProducts.Count>0)
            { 
                if (ProduceWaitProducts[0].ProductGotFlag)
                {
                    ProductProduced(ProduceWaitProducts[0]);
                }
                else
                { 
                    ProduceWaitProducts[0].StartProduce(time);
                }
            }
        }
        public abstract void GetPaidForProducts(GlobalResourceStock stock);
        
        public void CancelProduce(GlobalResourceStock stock)
        {
            if (ProduceWaitProducts!=null && ProduceWaitProducts.Count > 0)
            { 
                foreach (M product in ProduceWaitProducts)
                {
                    if (product.ProducePrice.PricePaidFlag)
                    { 
                        stock.ReturnPayForGlobalResourceStock(product.ProducePrice);
                        product.ResetCostInResurse();
                    }                    
                }
                foreach (M product in ProductsWaitPaid)
                {
                    product.GetBackResurse(stock.GlobalResStock);
                    product.ResetCostInResurse();
                }
                ProduceWaitProducts = new List<M>();
            }
        }
    }
}
