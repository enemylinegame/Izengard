using System;

namespace ResourceMarket
{
    public interface IMarketItem
    {
        public IMarketItemData Data { get; }

        public int CurrentAmount { get; }
        public int BuyCost { get; }
        public int ExchangeCost { get; }

        public event Action<int> OnAmountChange;

        public void IncreaseAmount(int amount);

        public void DecreaseAmount(int amount);
        void RestoreValue();
    }
}
