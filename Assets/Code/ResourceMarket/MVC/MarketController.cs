using System.Collections.Generic;
using System.Linq;
using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController
    {
        private readonly Dictionary<ResourceType, MarketItemModel> _marketItems = new Dictionary<ResourceType, MarketItemModel>();
        private readonly ICustomer _customer;
        private readonly GlobalStock _stock;
        private readonly MarketView _view;

        public MarketController(
            ICustomer customer, 
            GlobalStock stock,
            MarketView view,
            MarketDataConfig marketData)
        {
            _customer = customer;
            _stock = stock;
            _view = view;

            foreach(var itemData in marketData.MarketItemsData)
            {
                _marketItems[itemData.ResourceType] = new MarketItemModel(itemData);
            }
            
            _view.InitView(_marketItems.Values.ToList(), OnByItem, OnSellItem);
        }

        private void OnByItem(ResourceType resourceType)
        {
            var item = _marketItems[resourceType];

            if (_customer.Gold >= item.ExchangeCost)
            {
                item.IncreaseAmount(10);
                _customer.RemoveGold(item.ExchangeCost);
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
            var item = _marketItems[resourceType];

            if (item.CurrentAmount > 10)
            {
                item.DecreaseAmount(10);
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
