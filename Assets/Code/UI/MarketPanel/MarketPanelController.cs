using System;
using System.Collections.Generic;
using ResourceMarket;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class MarketPanelController
    {
        private MarketPanel _panel;

        private TMP_Text _exchangeAmountText => _panel.ExchangeAmountText;
        private TMP_Text _statusText => _panel.StatusText;
        private TMP_Text _marketAmountText => _panel.MarketAmountText;
        private TMP_Text _timerText => _panel.TimerText;
        private GameObject _marketItemPrefab => _panel.MarketItemPrefab;
        
        private Button _byItemButton => _panel.ByItemButton;
        private Button _sellItemButton => _panel.SellItemButton;
        private Button _increaseExchangeButton => _panel.IncreaseExchangeButton;
        private Button _decreaseExchangeButton => _panel.DecreaseExchangeButton;
        private Button _closeMarketButton => _panel.CloseMarketButton;

        private ItemsContainerView _tierOneItems => _panel.TierOneItems;
        private ItemsContainerView _tierTwoItems => _panel.TierTwoItems;
        private ItemsContainerView _tierThreeItems => _panel.TierThreeItems;

        private MarketTradeInfoView _marketTradeInfo => _panel.MarketTradeInfo;

        private List<MarketItemView> _itemsViewList;
        private ResourceType _currentSelectedType;
        private Action _onResetTradeValue;
        
        public MarketPanelController(MarketPanelFactory factory)
        {
            _panel = factory.GetView(factory.UIElementsConfig.MarketPanel);
        }
        
        public void InitViewData(
            MarketTierData tierData,
            IList<IMarketItem> tierOneItems,
            IList<IMarketItem> tierTwoItems,
            IList<IMarketItem> tierThreeItems)
        {
            _statusText.text = "";
          
            _itemsViewList = new List<MarketItemView>();
            CreateTierOneItemsView(tierData, tierOneItems);
            CreateTierTwoItemsView(tierData, tierTwoItems);
            CreateTierThreeItemsView(tierData, tierThreeItems);

            InitTradeInfoView(tierOneItems, tierTwoItems, tierThreeItems);

            SetActive(false);
        }

        private void CreateTierOneItemsView(MarketTierData tierData, IList<IMarketItem> items)
        {
            _tierOneItems.Init(tierData.TierOneUnlockValue);

            foreach (var item in items)
            {
                GameObject objectView = UnityEngine.Object.Instantiate(_marketItemPrefab, _tierOneItems.ItemPlacement, false);
                MarketItemView itemView = objectView.GetComponent<MarketItemView>();
                itemView.Init(item, OnItemClicked);

                _itemsViewList.Add(itemView);
            }
        }

        private void CreateTierTwoItemsView(MarketTierData tierData, IList<IMarketItem> items)
        {
            _tierTwoItems.Init(tierData.TierTwoUnlockValue);

            foreach (var item in items)
            {
                GameObject objectView = UnityEngine.Object.Instantiate(_marketItemPrefab, _tierTwoItems.ItemPlacement, false);
                MarketItemView itemView = objectView.GetComponent<MarketItemView>();
                itemView.Init(item, OnItemClicked);

                _itemsViewList.Add(itemView);
            }
        }

        private void CreateTierThreeItemsView(MarketTierData tierData, IList<IMarketItem> items)
        {
            _tierThreeItems.Init(tierData.TierThreeUnlockValue);

            foreach (var item in items)
            {
                GameObject objectView = UnityEngine.Object.Instantiate(_marketItemPrefab, _tierThreeItems.ItemPlacement, false);
                MarketItemView itemView = objectView.GetComponent<MarketItemView>();
                itemView.Init(item, OnItemClicked);

                _itemsViewList.Add(itemView);
            }
        }

        private void OnItemClicked(IMarketItem item)
        {
            _currentSelectedType = item.Data.ResourceType;

            foreach (var itemView in _itemsViewList)
            {
                if (itemView.CompareType(_currentSelectedType))
                {
                    itemView.SetSelected(true);
                }
                else
                {
                    itemView.SetSelected(false);
                }
            }
            
            _onResetTradeValue?.Invoke();

            SetButtonsInteraction(true);
        }

        private void SetButtonsInteraction(bool state)
        {
            _byItemButton.interactable = state;
            _sellItemButton.interactable = state;
            _increaseExchangeButton.interactable = state;
            _decreaseExchangeButton.interactable = state;
        }

        private void InitTradeInfoView(
            IList<IMarketItem> tierOneItems,
            IList<IMarketItem> tierTwoItems,
            IList<IMarketItem> tierThreeItems)
        {
            var tradeInfoDataList = new List<IMarketItem>();
            tradeInfoDataList.AddRange(tierOneItems);
            tradeInfoDataList.AddRange(tierTwoItems);
            tradeInfoDataList.AddRange(tierThreeItems);

            _marketTradeInfo.InitView(tradeInfoDataList);
        }

        public void InitViewAction(
            Action<ResourceType> byItem,
            Action<ResourceType> sellItem,
            Action increaseTradeValue,
            Action decreaseTradeValue,
            Action resetTradeValue)
        {
            _byItemButton.onClick.AddListener(() => byItem?.Invoke(_currentSelectedType));
            _sellItemButton.onClick.AddListener(() => sellItem?.Invoke(_currentSelectedType));

            _increaseExchangeButton.onClick.AddListener(() => increaseTradeValue?.Invoke());
            _decreaseExchangeButton.onClick.AddListener(() => decreaseTradeValue?.Invoke());
            
            _onResetTradeValue = resetTradeValue;

            _closeMarketButton.onClick.AddListener(() => _panel.gameObject.SetActive(false));

            SetButtonsInteraction(false);
        }

        public void Deinit()
        {
            _byItemButton.onClick.RemoveAllListeners();
            _sellItemButton.onClick.RemoveAllListeners();
            _increaseExchangeButton.onClick.RemoveAllListeners();
            _decreaseExchangeButton.onClick.RemoveAllListeners();

            _onResetTradeValue = default;

            _closeMarketButton.onClick.RemoveAllListeners();

            foreach (var itemView in _itemsViewList)
            {
                itemView.Deinit();
            }
        }

        public void SetActive(bool state)
        {
            if (state)
            {
                _onResetTradeValue?.Invoke();
            }
            else
            {
                foreach (var itemView in _itemsViewList)
                {
                    itemView.SetSelected(false);
                }
                SetButtonsInteraction(false);
            }

            _panel.gameObject.SetActive(state);
        }

        public void UpdateStatus(string message)
        {
            _statusText.text = message;
        }

        public void UpdateMarketAmount(int marketAmount)
        {
            _marketAmountText.text = marketAmount.ToString();

            _tierOneItems.CheckBlockState(marketAmount);
            _tierTwoItems.CheckBlockState(marketAmount);
            _tierThreeItems.CheckBlockState(marketAmount);
        }

        public void UpdateTimerTime(float time)
        {
            int hours = (int)(time / 3600);
            int minutes = (int)((time % 3600) / 60);
            int seconds = (int)(time % 60);

            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            _timerText.text = $"Restock in: {formattedTime}";
        }

        public void UpdateGold(int amount)
            => _marketTradeInfo.UpdateGold(amount);

        public void UpdateTradeValue(int tradeValue)
        {
            _exchangeAmountText.text = tradeValue.ToString();
        }
    }
}