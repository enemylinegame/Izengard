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
        
        [SerializeField] private Button _onClickButton;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _unselectedBackground;

        private IMarketItem _marketItem;

        private Action<IMarketItem> _onclickItem;

        public void Init(IMarketItem marketItem, Action<IMarketItem> selectItemAction)
        {
            _marketItem = marketItem;
            _marketItem.OnAmountChange += UpdateAmount;
            SetInfoData(_marketItem);

            _onclickItem = selectItemAction;

            _onClickButton.onClick.AddListener(ItemClick);
        }

        private void SetInfoData(IMarketItem marketItem)
        {
            _itemNameText.text = marketItem.Data.Name;
            _itemByCostText.text = $"Buy: {marketItem.BuyCost}";
            _itemSellCostText.text = $"Sell: {marketItem.ExchangeCost}";
            UpdateAmount(marketItem.CurrentAmount);
        }

        private void ItemClick()
        {
            _onclickItem?.Invoke(_marketItem);
        }

        public void SetSelected(bool isSelected) 
        {
            _selectedBackground.SetActive(isSelected);
            _unselectedBackground.SetActive(!isSelected);

        }

        public void Deinit()
        {
            _marketItem.OnAmountChange -= UpdateAmount;
            _marketItem = default;
            _onclickItem = default;
            _onClickButton.onClick.RemoveListener(ItemClick);
        }

        public void UpdateAmount(int currentAmount)
        {
            _itemAmountText.text = currentAmount.ToString();
        }
    }
}
