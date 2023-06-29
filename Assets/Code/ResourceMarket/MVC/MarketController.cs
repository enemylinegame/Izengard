using System.Collections.Generic;
using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController
    {
        private readonly List<MarketItemModel> _marketItems = new List< MarketItemModel>();

        private readonly GlobalStock _stock;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly MarketView _view;
        private int _currentGold;

        public MarketController(
            MarketView view,
            MarketDataConfig marketData,
            GlobalStock stock,
            IMarketDataProvider marketDataProvider)
        {
            _stock = stock;
            _stock.ResourceValueChanged += OnGoldChange;

            _marketDataProvider = marketDataProvider;
            _view = view;

            foreach(var itemData in marketData.MarketItemsData)
            {
                _marketItems.Add(new MarketItemModel(itemData, _marketDataProvider));
            }
               
            _view.InitView(_marketItems, OnByItem, OnSellItem);
            
            _marketDataProvider.OnMarketAmountChange += _view.UpdateMarketAmount;
            _view.UpdateMarketAmount(_marketDataProvider.MarketAmount);
        }

        private void OnByItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None) 
                return;

            var item = _marketItems.Find(r => r.ResourceType == resourceType);

            if (_currentGold >= item.BuyCost)
            {
                _stock.GetResourceFromStock(ResourceType.Gold, item.BuyCost);
                _stock.AddResourceToStock(resourceType, item.ExchangeAmount);

                item.DecreaseAmount(item.ExchangeAmount);

                _view.UpdateStatus("");
            }
            else 
            {
                _view.UpdateStatus("Нужно больше золота");
            }
        }

        private void OnSellItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None)
                return;

            var item = _marketItems.Find(r => r.ResourceType == resourceType);

            if (_stock.CheckResourceInStock(resourceType, item.ExchangeAmount) == false)
            {
                _stock.GetResourceFromStock(resourceType, item.ExchangeAmount);
                _stock.AddResourceToStock(ResourceType.Gold, item.ExchangeCost);

                item.IncreaseAmount(item.ExchangeAmount);

                _view.UpdateStatus("");
            }
            else
            {
                _view.UpdateStatus(item.ErrorMessage);
            }
        }

        private void OnGoldChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Gold) 
                return;

            _currentGold = value;
            _view.UpdateGold(_currentGold);
        }
    }
}
