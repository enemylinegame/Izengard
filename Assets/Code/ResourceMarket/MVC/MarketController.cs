using System.Collections.Generic;
using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController : IOnUpdate
    {
        private readonly List<IMarketItem> _marketItems = new List<IMarketItem>();

        private readonly MarketView _view;
        private readonly MarketCustomerController _marketCustomer;
        private readonly GlobalStock _stock;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IMarketItemFactory _itemFactory;
        
        private float _timer = 0f;
        private float _restoreDelay;
        
        private int _currentGold;

        public MarketController(
            MarketView view,
            MarketDataConfig marketData,
            MarketCustomerController marketCustomer,
            GlobalStock stock,
            IMarketDataProvider marketDataProvider)
        {
            _view = view;
            _marketCustomer = marketCustomer;

            _stock = stock;
            _stock.ResourceValueChanged += OnGoldChange;
            _stock.ResourceValueChanged += _marketCustomer.UpdateResourceAmount;

            _marketDataProvider = marketDataProvider;

            _itemFactory = new MarketItemFactory(marketData, marketDataProvider);

            var tierOneItems = _itemFactory.CreateTierOneItems();
            _marketItems.AddRange(tierOneItems);
            var tierTwoItems = _itemFactory.CreateTierTwoItems();
            _marketItems.AddRange(tierTwoItems);
            var tierthreeItems = _itemFactory.CreateTierThreeItems();
            _marketItems.AddRange(tierthreeItems);

            _view.InitView(
                marketData.MarketTierData, 
                tierOneItems, 
                tierTwoItems, 
                tierthreeItems, 
                OnBuyItem, OnSellItem);
            
            _marketDataProvider.OnMarketAmountChange += _view.UpdateMarketAmount;
            _view.UpdateMarketAmount(_marketDataProvider.MarketAmount);

            _restoreDelay = marketData.MarketRestoreValueDelay;
        }

        private void OnGoldChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Gold)
                return;

            _currentGold = value;
            _marketCustomer.UpdateCustomerGold(_currentGold);
        }

        private void OnBuyItem(ResourceType resourceType, int exchange)
        {
            if (resourceType == ResourceType.None) 
                return;

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_currentGold >= item.BuyCost)
            {
                _stock.GetResourceFromStock(ResourceType.Gold, item.BuyCost);
                _stock.AddResourceToStock(resourceType, exchange);

                item.DecreaseAmount(exchange);

                _view.UpdateStatus("");
            }
            else 
            {
                _view.UpdateStatus("Нужно больше золота");
            }
        }

        private void OnSellItem(ResourceType resourceType, int exchange)
        {
            if (resourceType == ResourceType.None)
                return;

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_stock.CheckResourceInStock(resourceType, exchange) == false)
            {
                _stock.GetResourceFromStock(resourceType, exchange);
                _stock.AddResourceToStock(ResourceType.Gold, item.ExchangeCost);

                item.IncreaseAmount(exchange);

                _view.UpdateStatus("");
            }
            else
            {
                _view.UpdateStatus(item.Data.ErrorMessage);
            }
        }

        public void ShowView()
        {
            _view.Show();
        }


        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer >= _restoreDelay)
            {
                RestoreItemValues();
                _timer = 0f;
            }
           
        }

        private void RestoreItemValues()
        {
            foreach(var item in _marketItems)
            {
                item.RestoreValue();
            }
        }
    }
}
