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
        [SerializeField] private TMP_Text _marketAmountText;

        [SerializeField] private MarketItemView _wood;
        [SerializeField] private MarketItemView _iron;
        
        public void InitView(
            List<IMarketItem> items,
            Action<ResourceType> byItem, 
            Action<ResourceType> sellItem)
        {     
            InitItemsView(items, byItem, sellItem);
        }

        private void InitItemsView(List<IMarketItem> items, Action<ResourceType> buyItem, Action<ResourceType> sellItem)
        {
            foreach (var item in items)
            {
                switch (item.Data.ResourceType)
                {
                    default:
                        break;
                    case ResourceType.Wood:
                        {
                            _wood.Init(item, buyItem, sellItem);
                            break;
                        }
                    case ResourceType.Iron:
                        {
                            _iron.Init(item, buyItem, sellItem);
                            break;
                        }
                }
            }
        }

        public void UpdateGold(int currentGold)
        {
            _goldText.text = $"Gold: {currentGold}";
        }

        public void UpdateStatus(string message)
        {
            _statusText.text = message;
        }

        public void UpdateMarketAmount(int marketAmount)
        {
            _marketAmountText.text = $"Markets: {marketAmount}";
        }
    }
}


