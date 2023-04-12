using System;
using UnityEngine;

namespace ResourceSystem.SupportClases
{
    [Serializable]
    public class  ResourcePriceModel
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private float _cost;

        public ResourceType ResourceType => _resourceType;

        public float Cost
        {
            get => _cost;
        }

        public ResourcePriceModel(ResourceType resourceType, float cost)
        {
            _resourceType = resourceType;
            _cost = cost;
        }
    }
}