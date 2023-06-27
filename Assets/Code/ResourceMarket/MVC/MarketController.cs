using ResourceSystem;

namespace ResourceMarket
{
    public sealed class MarketController
    {
        private readonly ICustomer _customer;
        private readonly GlobalStock _stock;

        public MarketController(
            ICustomer customer, 
            GlobalStock stock)
        {
            _customer = customer;
            _stock = stock;
        }

        private void OnByItem(ResourceType resourceType)
        {

        }

        private void OnSellItem(ResourceType resourceType)
        {

        }

    }
}
