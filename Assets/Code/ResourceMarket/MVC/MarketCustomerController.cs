using System;
using System.Collections.Generic;
using ResourceSystem;

namespace ResourceMarket
{
    public class MarketCustomerController
    {
        private MarketCustomerView _view;

        public MarketCustomerController(
            MarketCustomerView view,
            List<ResourceConfig> resourceConfigs)
        {
            _view = view;
            _view.InitView(resourceConfigs);
        }

        public void UpdateCustomerView(List<ResourceType> resources) 
            => _view.Display(resources);

        public void UpdateCustomerGold(int amount) 
            => _view.UpdateGold(amount);

        internal void UpdateResourceAmount(ResourceType resourceType, int value)
        {
            _view.UpdateResource(resourceType, value);
        }
    }
}
