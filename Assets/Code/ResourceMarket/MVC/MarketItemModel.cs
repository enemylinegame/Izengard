using System;
using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    public class MarketItemModel
    {
        private readonly IMarketDataProvider _marketDataProvider;

        private ResourceType _resourceType;
        private int _exchangeAmount;
        private int _exchangeRate;
        private int _exchangeCoef;
        private int _minExchange;
        private int _currentAmount;

        private string _name;
        private string _errorMessage;

        #region Public Fields

        public ResourceType ResourceType => _resourceType;
        public int ExchangeAmount => _exchangeAmount;
        public int CurrentAmount 
        {
            get => _currentAmount;
            private set
            {
                if(_currentAmount != value)
                {
                    _currentAmount = Mathf.Clamp(value, 0, int.MaxValue);
                }
            }
        }

        public int BuyCost
        {
            get => _exchangeAmount * _exchangeRate;
        }

        public int ExchangeCost
        {
            get => _exchangeAmount * (int)(_exchangeRate * (_exchangeCoef + _marketDataProvider.MarketCoef * _marketDataProvider.MarketAmount));
        }

        public string Name => _name;
        public string ErrorMessage => _errorMessage;

        public event Action<int> OnAmountChange;

        #endregion

        public MarketItemModel(
            MarketItemData itemData, 
            IMarketDataProvider marketDataProvider)
        {
            _resourceType = itemData.ResourceType;
            _exchangeAmount = itemData.ExchangeAmount;
            _exchangeRate = itemData.ExchangeRate;
            _minExchange = itemData.MinExchange;

            _currentAmount = itemData.InitialAmount;

            _name = itemData.Name;
            _errorMessage = itemData.ErrorMessage;

            _marketDataProvider = marketDataProvider;
        }

        public void IncreaseAmount(int amount)
        {
            CurrentAmount += amount;
            OnAmountChange?.Invoke(_currentAmount);
        }

        public void DecreaseAmount(int amount)
        {
            CurrentAmount -= amount;
            OnAmountChange?.Invoke(_currentAmount);
        }
    }
}
