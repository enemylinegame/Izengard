using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.UI;
using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    public sealed class MarketController : IOnController, IOnUpdate, IOnStart, IDisposable
    {
        private readonly List<IMarketItem> _marketItems;

        private readonly UIPanelsInitialization _uiPanels;
        private readonly MarketPanelController _marketPanel;
        private readonly GlobalStock _stock;
        private readonly BuildingFactory _buildingsFactory;

        private readonly IMarketDataProvider _marketDataProvider;
        private readonly IMarketItemFactory _itemFactory;

        private readonly TimeRemaining _resetTimer;

        private int _currentGold;
        private int _tradeValue;

        private float _restoreTimer;
        public float RestoreTimer 
        {
            get => _restoreTimer;
            set
            {
                if (_restoreTimer != value)
                {
                    _restoreTimer = value;

                    _marketPanel.UpdateTimerTime(_restoreTimer);
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
                    _marketPanel.UpdateTradeValue(_tradeValue);
                }
            }
        }

        public MarketController(UIPanelsInitialization uiPanels, GlobalStock stock, BuildingFactory buildingFactory,
            MarketDataConfig marketData)
        {
            _uiPanels = uiPanels;
            _marketItems = new List<IMarketItem>();

            _marketPanel = uiPanels.MarketPanelController;

            _stock = stock;
            _stock.ResourceValueChanged += OnGoldChange;

            _buildingsFactory = buildingFactory;
            _buildingsFactory.OnBuildingsChange += OnAddMarkets;

            _marketDataProvider = new MarketDataProvider(marketData.MarketCoef);
            _marketDataProvider.OnMarketAmountChange += _marketPanel.UpdateMarketAmount;

            _itemFactory = new MarketItemFactory(marketData, _marketDataProvider);

            var tierOneItems = _itemFactory.CreateTierOneItems();
            _marketItems.AddRange(tierOneItems);
            var tierTwoItems = _itemFactory.CreateTierTwoItems();
            _marketItems.AddRange(tierTwoItems);
            var tierthreeItems = _itemFactory.CreateTierThreeItems();
            _marketItems.AddRange(tierthreeItems);

            _marketPanel.InitViewData(marketData.MarketTierData, tierOneItems, tierTwoItems, tierthreeItems);
            _marketPanel.InitViewAction(OnBuyItem, OnSellItem, OnIncreaseTradeValue, OnDecreaseTradeValue, ResetTradeValue);
            _marketPanel.UpdateGold(_stock.GetAvailableResourceAccount(ResourceType.Gold));
            _marketPanel.UpdateMarketAmount(_marketDataProvider.MarketAmount);

            _resetTimer = new TimeRemaining(RestoreValues, marketData.MarketRestoreValueDelay, true);
        }

        private void OnGoldChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Gold)
                return;

            _currentGold = value;
            _marketPanel.UpdateGold(_currentGold);
        }

        private void OnAddMarkets(BuildingTypes type, bool isMarketBuild)
        {
            if (type != BuildingTypes.ResourceMarket)
                return;

            if (isMarketBuild)
            {
                _marketDataProvider.AddMarket();
            }
            else
            {
                _marketDataProvider.RemoveMarket();
            }
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

                _marketPanel.UpdateStatus("");
            }
            else 
            {
                _marketPanel.UpdateStatus("Нужно больше золота");
            }
        }

        private void OnSellItem(ResourceType resourceType)
        {
            if (resourceType == ResourceType.None)
                return;

            var item = _marketItems.Find(r => r.Data.ResourceType == resourceType);

            if (_stock.CheckResourceInStock(resourceType, item.Data.ExchangeAmount * TradeVale))
            {
                _stock.GetResourceFromStock(resourceType, item.Data.ExchangeAmount * TradeVale);
                _stock.AddResourceToStock(ResourceType.Gold, item.ExchangeCost * TradeVale);

                item.IncreaseAmount(item.Data.ExchangeAmount * TradeVale);

                _marketPanel.UpdateStatus("");
            }
            else
            {
                _marketPanel.UpdateStatus(item.Data.ErrorMessage);
            }
        }

        private void OnIncreaseTradeValue() 
            => TradeVale++;

        private void OnDecreaseTradeValue() 
            => TradeVale--;

        private void ResetTradeValue() 
            => TradeVale = 1;

        

        private void RestoreValues()
        {
            foreach (var item in _marketItems)
            {
                item.RestoreValue();
            }
            ResetTradeValue();
        }

        public void OnStart()
        {
            TimersHolder.AddTimer(_resetTimer);
        }
  
        public void OnUpdate(float deltaTime)
        {
            RestoreTimer = _resetTimer.TimeLeft;
        }
  
        public void Dispose()
        {
            _stock.ResourceValueChanged -= OnGoldChange;
            _buildingsFactory.OnBuildingsChange -= OnAddMarkets;
            _marketDataProvider.OnMarketAmountChange -= _marketPanel.UpdateMarketAmount;

            _uiPanels.RightPanelController.Dispose();

            TimersHolder.RemoveTimer(_resetTimer);

            _marketPanel.Deinit();
        }  
    }
}
