using System.Collections.Generic;
using ResourceSystem;
using TMPro;
using UnityEngine;

namespace ResourceMarket
{
    public class CustomerDataView : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _itemContainer;
        
        private List<CustomerItemView> _customerItems;

        public void InitView(List<ResourceConfig> resourceConfigs) 
        {
            _customerItems = new List<CustomerItemView>();

            for (int i =0; i < resourceConfigs.Count; i++)
            {
                if(resourceConfigs[i].ResourceType != ResourceType.Gold)
                {
                    _customerItems.Add(CreateItemView(resourceConfigs[i]));
                }              
            }
        }

        private CustomerItemView CreateItemView(ResourceConfig resource)
        {
            GameObject objectView = Instantiate(_itemPrefab, _itemContainer, false);
            CustomerItemView itemView = objectView.GetComponent<CustomerItemView>();

            itemView.InitView(resource);
            return itemView;
        }

        public void UpdateResource(ResourceType resourceType, int value)
        {
            var item = _customerItems.Find(cItem => cItem.ResourceType == resourceType);
            if(item != null)
            {
                item.ChangeAmount(value);
            }
        }
    }
}
