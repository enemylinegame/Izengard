﻿using System;
using System.Collections.Generic;
using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController
    {
        private readonly List<IMarketItem> _marketItems = new List<IMarketItem>();

        private readonly MarketView _view;
        private readonly MarketCustomerController _marketCustomer;
        private readonly GlobalStock _stock;
        private readonly IMarketDataProvider _marketDataProvider;

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
         
            foreach(var itemData in marketData.MarketItemsData)
            {
                if(itemData.TierType == TierType.Tier1)
                {
                    _marketItems.Add(new TierOneItemModel(itemData, _marketDataProvider));
                }
                else if(itemData.TierType == TierType.Tier1)
                {
                    _marketItems.Add(new TierTwoItemModel(itemData, _marketDataProvider));
                }               
            }
               
            _view.InitView(_marketItems, OnBuyItem, OnSellItem);
            
            _marketDataProvider.OnMarketAmountChange += _view.UpdateMarketAmount;
            _view.UpdateMarketAmount(_marketDataProvider.MarketAmount);
        }

        private void OnGoldChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Gold)
                return;

            _currentGold = value;
            _marketCustomer.UpdateCustomerGold(_currentGold);
        }

        private void OnBuyItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None) 
                return;

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_currentGold >= item.BuyCost)
            {
                _stock.GetResourceFromStock(ResourceType.Gold, item.BuyCost);
                _stock.AddResourceToStock(resourceType, item.Data.ExchangeAmount);

                item.DecreaseAmount(item.Data.ExchangeAmount);

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

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_stock.CheckResourceInStock(resourceType, item.Data.ExchangeAmount) == false)
            {
                _stock.GetResourceFromStock(resourceType, item.Data.ExchangeAmount);
                _stock.AddResourceToStock(ResourceType.Gold, item.ExchangeCost);

                item.IncreaseAmount(item.Data.ExchangeAmount);

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
    }
}
