using System.Collections.Generic;
using ResourceSystem;

namespace ResourceMarket
{
    public class CustomerController
    {
        private readonly CustomerDataView _view;
        private readonly GlobalStock _stock;
        public CustomerController(
            CustomerDataView view,
            List<ResourceConfig> resourceConfigs,
            GlobalStock stock)
        {
            _view = view;
            _stock = stock;

            _stock.ResourceValueChanged += UpdateResourceAmount;

            _view.InitView(resourceConfigs);
        }

        internal void UpdateResourceAmount(ResourceType resourceType, int value)
        {
            _view.UpdateResource(resourceType, value);
        }
    }
}
