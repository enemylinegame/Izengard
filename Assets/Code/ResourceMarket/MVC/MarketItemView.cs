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
        [SerializeField] private TMP_Text _itemByCostText;
        [SerializeField] private TMP_Text _itemSellCostText;

        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;

        private IMarketItem _marketItem;

        private Action<ResourceType> _onByItem;
        private Action<ResourceType> _onSellItem;

        public void Init(IMarketItem marketItem, Action<ResourceType> buyItemAction, Action<ResourceType> sellItemAction)
        {
            _marketItem = marketItem;
            _marketItem.OnAmountChange += UpdateAmount;
            SetInfoData(_marketItem);

            _onByItem = buyItemAction;
            _onSellItem = sellItemAction;

            _byItemButton.onClick.AddListener(ByItem);
            _sellItemButton.onClick.AddListener(SellItem);
        }

        private void SetInfoData(IMarketItem marketItem)
        {
            _itemNameText.text = marketItem.Data.Name;
            _itemByCostText.text = $"By {marketItem.Data.ExchangeAmount} for {marketItem.BuyCost} gold";
            _itemSellCostText.text = $"Sell {marketItem.Data.ExchangeAmount} for {marketItem.ExchangeCost} gold";
            UpdateAmount(marketItem.CurrentAmount);
        }

        private void ByItem()
        {
            _onByItem?.Invoke(_marketItem.Data.ResourceType);
        }

        private void SellItem()
        {
            _onSellItem?.Invoke(_marketItem.Data.ResourceType);
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
