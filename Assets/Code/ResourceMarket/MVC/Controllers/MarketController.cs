using System;
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    public sealed class MarketController : IOnUpdate
    {
        private readonly List<IMarketItem> _marketItems = new List<IMarketItem>();

        private readonly MarketView _view;
        private readonly GlobalStock _stock;
        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IMarketItemFactory _itemFactory;
        
        private int _currentGold;
        private int _tradeValue;
        private float _restoreTime;
        private float _restoreTimer = 0f;

        public float RestoreTimer 
        {
            get => _restoreTimer;
            set
            {
                if (_restoreTimer != value)
                {
                    _restoreTimer = value;

                    _view.UpdateTimerTime(_restoreTimer);
                    OnTimerUpdate?.Invoke(_restoreTimer);
                }
            }
        }
   
        public int TradeVale
        {
            get => _tradeValue;
            set
            {
                if (_tradeValue != value)
                {
                    _tradeValue = Mathf.Clamp(value, 1, int.MaxValue);
                    _view.UpdateTradeValue(_tradeValue);
                }
            }
        }

        public event Action<float> OnTimerUpdate;

        public MarketController(
            MarketView view,
            MarketDataConfig marketData,
            GlobalStock stock,
            IMarketDataProvider marketDataProvider)
        {
            _view = view;

            _stock = stock;
            _stock.ResourceValueChanged += OnGoldChange;

            _marketDataProvider = marketDataProvider;

            _itemFactory = new MarketItemFactory(marketData, marketDataProvider);

            var tierOneItems = _itemFactory.CreateTierOneItems();
            _marketItems.AddRange(tierOneItems);
            var tierTwoItems = _itemFactory.CreateTierTwoItems();
            _marketItems.AddRange(tierTwoItems);
            var tierthreeItems = _itemFactory.CreateTierThreeItems();
            _marketItems.AddRange(tierthreeItems);

            _view.InitViewData(marketData.MarketTierData, tierOneItems, tierTwoItems, tierthreeItems);
            _view.InitViewAction(OnBuyItem, OnSellItem, OnIncreaseTradeValue, OnDecreaseTradeValue, ResetTradeValue);
            
            _marketDataProvider.OnMarketAmountChange += _view.UpdateMarketAmount;
            _view.UpdateMarketAmount(_marketDataProvider.MarketAmount);

            _restoreTime = marketData.MarketRestoreValueDelay;
            RestoreTimer = _restoreTime;
        }

        private void OnGoldChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Gold)
                return;

            _currentGold = value;
            _view.UpdateGold(_currentGold);
        }

        private void OnBuyItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None) 
                return;

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_currentGold >= item.BuyCost * TradeVale)
            {
                _stock.GetResourceFromStock(ResourceType.Gold, item.BuyCost * TradeVale);
                _stock.AddResourceToStock(resourceType, item.Data.ExchangeAmount * TradeVale);

                item.DecreaseAmount(item.Data.ExchangeAmount * TradeVale);

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

            if (_stock.CheckResourceInStock(resourceType, item.Data.ExchangeAmount * TradeVale) == false)
            {
                _stock.GetResourceFromStock(resourceType, item.Data.ExchangeAmount * TradeVale);
                _stock.AddResourceToStock(ResourceType.Gold, item.ExchangeCost * TradeVale);

                item.IncreaseAmount(item.Data.ExchangeAmount * TradeVale);

                _view.UpdateStatus("");
            }
            else
            {
                _view.UpdateStatus(item.Data.ErrorMessage);
            }
        }

        private void OnIncreaseTradeValue() 
            => TradeVale++;

        private void OnDecreaseTradeValue() 
            => TradeVale--;

        private void ResetTradeValue() 
            => TradeVale = 1;

        public void ShowView()
        {
            _view.Show();
        }

        public void OnUpdate(float deltaTime)
        {
            RestoreTimer -= deltaTime;

            if (RestoreTimer <= 0)
            {
                RestoreValues();
                RestoreTimer = _restoreTime;
            }
        }

        private void RestoreValues()
        {
            foreach(var item in _marketItems)
            {
                item.RestoreValue();
            }
            ResetTradeValue();
        }
    }
}
