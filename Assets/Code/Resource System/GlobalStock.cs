using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    public class GlobalStock
    {
        private List<ResourceHolder> _resourceHolders;
        public GlobalStock(List<ResourceConfig> resourceConfigs)
        {
            _resourceHolders = new List<ResourceHolder>();
            InitHolders(resourceConfigs);
        }

        private void InitHolders(List<ResourceConfig> resourceConfigs)
        {
            foreach (ResourceConfig resourceConfig in resourceConfigs)
            {
                if (_resourceHolders.Exists(x => x.ResourceType == resourceConfig.ResourceType))
                {
                    Debug.LogError($"Duplicate in resourceList for {resourceConfig.ResourceType}");
                    continue;
                }
                ResourceHolder resourceHolder  = new ResourceHolder(resourceConfig.ResourceType,resourceConfig.MaxHoldedAmount);
                _resourceHolders.Add(resourceHolder);
            }
        }

        public void AddResourceToStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = _resourceHolders.Find(x => x.ResourceType == resourceType);
            resourceHolder.AddResource(value);
        }
        public void GetResourceFromStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = _resourceHolders.Find(x => x.ResourceType == resourceType);
            resourceHolder.RemoveResource(value);
        }

        public bool CheckResourceInStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = _resourceHolders.Find(x => x.ResourceType == resourceType);
            return resourceHolder.CheckAmount(value);
        }
    }
}