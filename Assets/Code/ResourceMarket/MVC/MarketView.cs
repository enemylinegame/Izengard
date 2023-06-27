using System;
using System.Collections.Generic;
using ResourceSystem;
using TMPro;
using UnityEngine;

namespace ResourceMarket 
{
    public sealed class MarketView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _statusText;

        [SerializeField] private MarketItemView _wood;
        [SerializeField] private MarketItemView _iron;
        
        public void InitView(
            List<MarketItemModel> items,
            Action<ResourceType> byItem, 
            Action<ResourceType> sellItem)
        {     
            InitItemsView(items, byItem, sellItem);
        }

        private void InitItemsView(List<MarketItemModel> items, Action<ResourceType> byItem, Action<ResourceType> sellItem)
        {
            foreach (var item in items)
            {
                switch (item.ResourceType)
                {
                    default:
                        break;
                    case ResourceType.Wood:
                        {
                            _wood.Init(item, byItem, sellItem);
                            break;
                        }
                    case ResourceType.Iron:
                        {
                            _iron.Init(item, byItem, sellItem);
                            break;
                        }
                }
            }
        }

        public void UpdateGold(int currentGold)
        {
            _goldText.text = currentGold.ToString();
        }

        public void UpdateStatus(string message)
        {
            _statusText.text = message;
        }
    }
}


