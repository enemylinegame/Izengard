using UnityEngine;

namespace ResourceMarket
{
    public sealed class MarketEnetryPoint : MonoBehaviour
    {
        [SerializeField] private MarketView _marketView;
        [SerializeField] private MarketDataConfig _marketData;
        [SerializeField] private int _initialGold = 1000;

        private MarketController _marketController;

        private void Start()
        {
            var customer = new TestCustomer(_initialGold);
            var marketProvider = new TestMarketDataProvider(_marketData.MarketCoef, _marketData.MarketBuildings);
            _marketController = new MarketController(customer, marketProvider, _marketView, _marketData);
        }
    }
}
