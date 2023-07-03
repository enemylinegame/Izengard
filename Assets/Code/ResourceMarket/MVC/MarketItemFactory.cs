using System.Collections.Generic;

namespace ResourceMarket
{
    public interface IMarketItemFactory 
    {
        public IList<IMarketItem> CreateTierOneItems();
        public IList<IMarketItem> CreateTierTwoItems();
        public IList<IMarketItem> CreateTierThreeItems();
    }

    public sealed class MarketItemFactory : IMarketItemFactory
    {
        private readonly MarketDataConfig _config;
        private readonly IMarketDataProvider _marketDataProvider;
        public MarketItemFactory(MarketDataConfig config, IMarketDataProvider marketDataProvider)
        {
            _config = config;
            _marketDataProvider = marketDataProvider;
        }

        public IList<IMarketItem> CreateTierOneItems()
        {
            return GetItemsByTier(TierType.Tier1);
        }

        public IList<IMarketItem> CreateTierThreeItems()
        {
            return GetItemsByTier(TierType.Tier2);
        }

        public IList<IMarketItem> CreateTierTwoItems()
        {
            return GetItemsByTier(TierType.Tier3);
        }

        private IList<IMarketItem> GetItemsByTier(TierType tier)
        {
            var result = new List<IMarketItem>();

            foreach (var itemData in _config.MarketItemsData)
            {
                if (itemData.TierType == tier)
                {
                    result.Add(new TierOneItemModel(itemData, _marketDataProvider));
                }
            }

            return result;
        }
    }
}
