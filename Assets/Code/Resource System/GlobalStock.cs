using System;
using System.Collections.Generic;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    public class GlobalStock
    {
        private List<ResourceHolder> _resourceHolders;

        public event Action<ResourceType, int> ResourceValueChanged;
        
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

        public void TopPanelUIBind()
        {
            foreach (ResourceHolder resourceHolder in _resourceHolders)
            {
                ResourceValueChanged?.Invoke(resourceHolder.ResourceType, resourceHolder.CurrentAmount);
            }
        }

        public void AddResourceToStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = 
                _resourceHolders.Find(x => x.ResourceType == resourceType);

            if (null == resourceHolder)
            {
                Debug.LogError("Unknown resource Type!");
                return;
            }

            resourceHolder.AddResource(value);
            ResourceCountChanged(resourceType, resourceHolder.CurrentAmount);
        }
        public void GetResourceFromStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = _resourceHolders.Find(x => x.ResourceType == resourceType);
            resourceHolder.RemoveResource(value);
            ResourceCountChanged(resourceType, resourceHolder.CurrentAmount);
        }

        public bool CheckResourceInStock(ResourceType resourceType, int value)
        {
            ResourceHolder resourceHolder = _resourceHolders.Find(x => x.ResourceType == resourceType);
            return resourceHolder.CheckAmount(value);
        }

        private void ResourceCountChanged(ResourceType resourceType, int value)
        {
            ResourceValueChanged?.Invoke(resourceType, value);
        }
    }
}