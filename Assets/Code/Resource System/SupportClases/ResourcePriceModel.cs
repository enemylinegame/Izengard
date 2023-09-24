using System;
using UnityEngine;

namespace ResourceSystem.SupportClases
{
    [Serializable]
    public class  ResourcePriceModel
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _cost;

        public ResourceType ResourceType => _resourceType;

        public int Cost=> _cost;
       

        public ResourcePriceModel(ResourceType resourceType, int cost)
        {
            _resourceType = resourceType;
            _cost = cost;
        }
    }
}