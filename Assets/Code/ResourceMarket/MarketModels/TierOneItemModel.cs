using System;

namespace ResourceMarket
{
    public sealed class TierOneItemModel : BaseItemModel
    {
        private readonly IMarketDataProvider _marketDataProvider;

        public override int BuyCost
        {
            get => _data.ExchangeAmount * _data.ExchangeRate;
        }

        public override int ExchangeCost
        {
            get
            {
                var exchangeAdditionCoef = _data.ExchangeRate * (_data.ExchangeCoef + _marketDataProvider.MarketCoef * _marketDataProvider.MarketAmount);
                return _data.ExchangeAmount * (int)exchangeAdditionCoef;
            }
        }

        public TierOneItemModel(
            IMarketItemData data,
            IMarketDataProvider marketDataProvider) : base(data)
        {
            _marketDataProvider = marketDataProvider;
        }

        public override void IncreaseAmount(int amount)
        {
            CurrentAmount += amount;
        }

        public override void DecreaseAmount(int amount)
        {
            CurrentAmount -= amount;
        }

        public override void RestoreValue()
        {
            CurrentAmount = Data.InitialAmount;
        }
    }
}
