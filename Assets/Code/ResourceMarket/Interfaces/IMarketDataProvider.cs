using System;

namespace ResourceMarket
{
    public interface IMarketDataProvider
    {
        public event Action<int> OnMarketAmountChange;
        public double MarketCoef { get; }
        public int MarketAmount { get; }

        public void AddMarket();
        public void RemoveMarket();
    }
}
