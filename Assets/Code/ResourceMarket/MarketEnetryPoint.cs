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
            _marketController = new MarketController(customer, null, _marketView, _marketData);
        }
    }
}
