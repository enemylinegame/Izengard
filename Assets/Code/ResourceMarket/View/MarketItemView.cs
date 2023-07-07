﻿using System;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketItemView :  MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _itemNameText;
        [SerializeField] private TMP_Text _itemAmountText;
        [SerializeField] private TMP_Text _itemExchangeAmountText;
        
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
            _icon.sprite = marketItem.Data.Icon;
            _itemNameText.text = marketItem.Data.Name;
            _itemExchangeAmountText.text = $"{marketItem.Data.ExchangeAmount}";
            UpdateAmount(marketItem.CurrentAmount);
        }

        private void ItemClick()
        {
            _onclickItem?.Invoke(_marketItem);
        }

        public bool CompareType(ResourceType type) 
            => _marketItem.Data.ResourceType == type; 

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
