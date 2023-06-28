using System.Collections.Generic;
using System.Linq;
using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController
    {
        private readonly Dictionary<ResourceType, MarketItemModel> _marketItems = new Dictionary<ResourceType, MarketItemModel>();

        private readonly ICustomer _customer;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly MarketView _view;

        public MarketController(
            ICustomer customer,
            IMarketDataProvider marketDataProvider,
            MarketView view,
            MarketDataConfig marketData)
        {
            _customer = customer;
            _marketDataProvider = marketDataProvider;
            _view = view;

            foreach(var itemData in marketData.MarketItemsData)
            {
                _marketItems[itemData.ResourceType] = new MarketItemModel(itemData, _marketDataProvider);
            }
               
            _view.InitView(_marketItems.Values.ToList(), OnByItem, OnSellItem);
            
            _marketDataProvider.OnMarketAmountChange += _view.UpdateMarketAmount;
            _view.UpdateMarketAmount(_marketDataProvider.MarketAmount);
        }

        private void OnByItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None) 
                return;

            var item = _marketItems[resourceType];

            if (_customer.Gold >= item.BuyCost)
            {
                item.IncreaseAmount(item.ExchangeAmount);
                _customer.RemoveGold(item.BuyCost);
                _view.UpdateGold(_customer.Gold);
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

            var item = _marketItems[resourceType];

            if (item.CurrentAmount > item.ExchangeAmount)
            {
                item.DecreaseAmount(item.ExchangeAmount);
                _customer.AddGold(item.ExchangeCost);
                _view.UpdateGold(_customer.Gold);
                _view.UpdateStatus("");
            }
            else
            {
                _view.UpdateStatus(item.ErrorMessage);
            }
        }

    }
}
