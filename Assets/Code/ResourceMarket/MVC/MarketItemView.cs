using System;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketItemView :  MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemNameText;
        [SerializeField] private TMP_Text _itemAmountText;
        [SerializeField] private TMP_Text _itemCostText;

        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;

        private MarketItemModel _marketItem;

        private Action<ResourceType> _onByItem;
        private Action<ResourceType> _onSellItem;

        public void Init(MarketItemModel marketItem, Action<ResourceType> byItemAction, Action<ResourceType> sellItemAction)
        {
            _marketItem = marketItem;
            _marketItem.OnAmountChange += UpdateAmount;
            SetInfoData(_marketItem);

            _onByItem = byItemAction;
            _onSellItem = sellItemAction;

            _byItemButton.onClick.AddListener(ByItem);
            _sellItemButton.onClick.AddListener(SellItem);
        }

        private void SetInfoData(MarketItemModel marketItem)
        {
            _itemNameText.text = marketItem.Name;
            _itemCostText.text = $"{marketItem.ExchangeAmount} for {marketItem.ExchangeCost} gold";
            UpdateAmount(marketItem.CurrentAmount);
        }

        private void ByItem()
        {
            _onByItem?.Invoke(_marketItem.ResourceType);
        }

        private void SellItem()
        {
            _onSellItem?.Invoke(_marketItem.ResourceType);
        }

        public void Deinit()
        {
            _marketItem.OnAmountChange -= UpdateAmount;
            _marketItem = default;

            _onByItem = default;
            _onSellItem = default;

            _byItemButton.onClick.RemoveListener(ByItem);
            _sellItemButton.onClick.RemoveListener(SellItem);
        }

        public void UpdateAmount(int currentAmount)
        {
            _itemAmountText.text = currentAmount.ToString();
        }

        private void OnDestroy()
        {
            Deinit();
        }
    }
}
