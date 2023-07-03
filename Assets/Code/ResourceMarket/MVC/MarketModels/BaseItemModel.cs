using System;
using UnityEngine;

namespace ResourceMarket
{
    public abstract class BaseItemModel : IMarketItem
    {
        protected readonly IMarketItemData _data;

        private int _currentAmount;

        public IMarketItemData Data => _data;

        public int CurrentAmount
        {
            get => _currentAmount;
            protected set
            {
                if (_currentAmount != value)
                {
                    _currentAmount = Mathf.Clamp(value, 0, int.MaxValue);
                    OnAmountChange?.Invoke(_currentAmount);
                }
            }
        }

        public abstract int BuyCost { get; }

        public abstract int ExchangeCost { get; }

        public event Action<int> OnAmountChange;

        public BaseItemModel(IMarketItemData data)
        {
            _data = data;

            CurrentAmount = data.InitialAmount;
        }

        public abstract void DecreaseAmount(int amount);

        public abstract void IncreaseAmount(int amount);

        public abstract void RestoreValue();
    }
}
