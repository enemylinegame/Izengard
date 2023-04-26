using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileResourceUIController
    {
        private const int MAX_RESOURCES = 5;
        private ResourcesLayoutUIView _resourcesLayoutUIView;
        private List<int> _resourceValueList;

        public TileResourceUIController(ResourcesLayoutUIView resourcesLayoutUIView)
        {
            _resourcesLayoutUIView = resourcesLayoutUIView;

            Init();
        }

        private void Init()
        {
            _resourceValueList = new List<int>();
            foreach (ResourceView res in _resourcesLayoutUIView.Resources)
            {
                AddNewLayoutElement(res);
            }
        }

        public void AddNewLayoutElement(ResourceView res)
        {
            _resourceValueList.Add(Convert.ToInt32(res.ResourceCurrentValueString));
            res.ResourceAddButton.onClick.AddListener(() => AddResource(res));
            res.ResourceRemoveButton.onClick.AddListener(() => RemoveResource(res));
        }

        private void AddResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceView.ResourceCurrentValueInt < MAX_RESOURCES)
            {
                resourceValue++;
            }
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }
        private void RemoveResource(ResourceView resourceView)
        {
            int resourceValue = resourceView.ResourceCurrentValueInt;
            if (resourceValue > 0)
            {
                resourceValue--;
            }
            resourceView.ResourceCurrentValueString = $"{resourceValue}";
        }

    }
}
