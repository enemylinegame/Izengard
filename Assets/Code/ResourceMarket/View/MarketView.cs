using System;
using System.Collections.Generic;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _exchangeAmountText;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_Text _marketAmountText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GameObject _marketItemPrefab;

        [Space(10)]
        [Header("Buttons")]
        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;
        [SerializeField] private Button _increaseExchangeButton;
        [SerializeField] private Button _decreaseExchangeButton;
        [SerializeField] private Button _closeMarketButton;

        [Space(10)]
        [Header("Items Tier Settings")]
        [SerializeField] private ItemsContainerView _tierOneItems;
        [SerializeField] private ItemsContainerView _tierTwoItems;
        [SerializeField] private ItemsContainerView _tierThreeItems;

        [SerializeField] private MarketTradeInfoView _marketTradeInfo;

        private List<MarketItemView> _itemsViewList;
        private ResourceType _currentSelectedType;
        private Action _onResetTradeValue;

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
                GameObject objectView = Instantiate(_marketItemPrefab, _tierOneItems.ItemPlacement, false);
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
                GameObject objectView = Instantiate(_marketItemPrefab, _tierTwoItems.ItemPlacement, false);
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
                GameObject objectView = Instantiate(_marketItemPrefab, _tierThreeItems.ItemPlacement, false);
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
            Action resetTradeValue,
            Action closeMarket)
        {
            _byItemButton.onClick.AddListener(() => byItem?.Invoke(_currentSelectedType));
            _sellItemButton.onClick.AddListener(() => sellItem?.Invoke(_currentSelectedType));

            _increaseExchangeButton.onClick.AddListener(() => increaseTradeValue?.Invoke());
            _decreaseExchangeButton.onClick.AddListener(() => decreaseTradeValue?.Invoke());
            
            _onResetTradeValue = resetTradeValue;

            _closeMarketButton.onClick.AddListener(() => closeMarket?.Invoke());

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

            gameObject.SetActive(state);
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


