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
        [SerializeField] private GameObject _marketItemPrefab;

        [Space(10)]
        [Header("Buttons")]
        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;

        [Space(10)]
        [Header("Items Tier Settings")]
        [SerializeField] private ItemsContainerView _tierOneItems;
        [SerializeField] private ItemsContainerView _tierTwoItems;
        [SerializeField] private ItemsContainerView _tierThreeItems;

        [SerializeField] private MarketCustomerView _customerView;

        private List<MarketItemView> _itemsViewList;
        public MarketCustomerView CustomerView => _customerView;
        private ResourceType _currentSelectedType;

        private int _exchangeAmount;

        private void Awake()
        {
            Hide();
        }

        public void InitView(
            MarketTierData tierData,
            IList<IMarketItem> tierOneItems,
            IList<IMarketItem> tierTwoItems,
            IList<IMarketItem> tierThreeItems,
            Action<ResourceType> byItem,
            Action<ResourceType> sellItem)
        {
            _statusText.text = "";
            _itemsViewList = new List<MarketItemView>();

            CreateTierOneItemsView(tierData, tierOneItems);
            CreateTierTwoItemsView(tierData, tierTwoItems);
            CreateTierThreeItemsView(tierData, tierThreeItems);

            _byItemButton.onClick.AddListener
                (
                    () => byItem?.Invoke(_currentSelectedType)
                );

            _sellItemButton.onClick.AddListener
                (
                    () => sellItem?.Invoke(_currentSelectedType)
                );
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

            _exchangeAmount = item.Data.ExchangeAmount;

            _exchangeAmountText.text = _exchangeAmount.ToString();
        }

        public void Deinit()
        {
            _byItemButton.onClick.RemoveAllListeners();
            _sellItemButton.onClick.RemoveAllListeners();
            foreach (var itemView in _itemsViewList)
            {
                itemView.Deinit();
            }
        }

        public void Show()
            => gameObject.SetActive(true);

        public void Hide()
            => gameObject.SetActive(false);

        public void UpdateStatus(string message)
        {
            _statusText.text = message;
        }

        public void UpdateMarketAmount(int marketAmount)
        {
            _marketAmountText.text = $"Markets: {marketAmount}";

            _tierOneItems.CheckBlockState(marketAmount);
            _tierTwoItems.CheckBlockState(marketAmount);
            _tierThreeItems.CheckBlockState(marketAmount);
        }
    }
}


