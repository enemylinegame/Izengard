using System;
using ResourceSystem;

namespace ResourceMarket
{
    public class MarketItemModel
    {
        private ResourceType _resourceType;
        private int _exchangeAmount;
        private int _exchangeRate;
        private int _minAmount;
        private int _currentAmount;

        private string _name;
        private string _errorMessage;

        public ResourceType ResourceType => _resourceType;
        public int CurrentAmount => _currentAmount;
        public int ExchangeAmount => _exchangeAmount;
        public int ExchangeCost => _exchangeAmount * _exchangeRate;

        public string Name => _name;
        public string ErrorMessage => _errorMessage;

        public event Action<int> OnAmountChange;

        public MarketItemModel(MarketItemData itemData)
        {
            _resourceType = itemData.ResourceType;
            _exchangeAmount = itemData.ExchangeAmount;
            _exchangeRate = itemData.ExchangeRate; 
            _currentAmount = itemData.InitialAmount;
            _minAmount = itemData.MinAmount;

            _name = itemData.Name;
            _errorMessage = itemData.ErrorMessage;
        }

        public void IncreaseAmount(int amount)
        {
            _currentAmount += amount;

            if (_currentAmount < _minAmount)
            {
                _currentAmount = _minAmount;
            }

            OnAmountChange?.Invoke(_currentAmount);
        }

        public void DecreaseAmount(int amount)
        {
            _currentAmount -= amount;
            OnAmountChange?.Invoke(_currentAmount);
        }
    }
}
