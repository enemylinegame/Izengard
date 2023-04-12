using UnityEngine;

namespace ResourceSystem
{ 
    [System.Serializable]
    public abstract class Product<T> where T:ScriptableObject
    {
        public T ObjectProduct => _objectProduct;
        public float ProduceValue => _produceValue;
        public float ProduceTime => _produceTime;
        public ResourceCost ProducePrice => _producePrice;
        public bool ProductGotFlag => _productGotFlag;

        [SerializeField] protected T _objectProduct;        
        [SerializeField] protected float _produceValue;
        [SerializeField] protected float _produceTime;
        [SerializeField] protected ResourceCost _producePrice;
        [SerializeField] protected float _currentTime;
        [SerializeField] protected bool _productGotFlag = false;

        public Product(T obj,float produceValue,float produceTime,ResourceCost costInResource)
        {
            _objectProduct = obj;
            _produceValue = produceValue;
            _produceTime = produceTime;
            _producePrice = costInResource;
            _currentTime = 0;

        }
        public Product(T obj, float produceValue, float produceTime)
        {
            _objectProduct = obj;
            _produceValue = produceValue;
            _produceTime = produceTime;
            _producePrice = null;
            _currentTime = 0;

        }
        public Product(Product<T> product)
        {
            _objectProduct = product.ObjectProduct;
            _produceValue = product.ProduceValue;
            _produceTime = product.ProduceTime;
            _producePrice = product.ProducePrice;
            _productGotFlag = product.ProductGotFlag;
            _producePrice.ResetPaid();

        }
        public void ChangeProduceValue(float value)
        {
            _produceValue = value;
        }
        public void ChangeProduceTime(float value)
        {
            _produceTime = value;
        }
        public void PaidCostForProduceProduct(ResourceStock stock)
        {
            _producePrice.GetNeededResource(stock);
            
        }
        public void PaidCostForProduceProduct(GlobalResourceStock stock)
        {
            stock.GetResurseForProduceFromGlobalStock(_producePrice);
            
        }
        public void GetBackResurse(ResourceStock stock)
        {
            _producePrice.GetBackResource(stock);
        }
        public void ResetCostInResurse()
        {
            _producePrice.ResetPaid();
            _currentTime = 0;
            _productGotFlag = false;
        }
        public void StartProduce(float time)
        {
            _currentTime += time;
            if (_currentTime >= ProduceTime)
            {
                ResetCostInResurse();
                _productGotFlag=true;
            }
        }


    }
}
