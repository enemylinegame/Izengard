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
        
        [Space(10)]
        [Header("Buttons")]
        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;


        [SerializeField] private MarketItemView _wood;
        [SerializeField] private MarketItemView _iron;

        [SerializeField] private MarketCustomerView _customerView;

        public MarketCustomerView CustomerView => _customerView;
        private ResourceType _currentSelectedType;

        private int _exchangeAmount;

        private void Awake()
        {
            Hide();
        }

        public void InitView(
            List<IMarketItem> items,
            Action<ResourceType> byItem, 
            Action<ResourceType> sellItem)
        {

            _statusText.text = "";

            _byItemButton.onClick.AddListener
                (
                    () => byItem?.Invoke(_currentSelectedType)
                );

            _sellItemButton.onClick.AddListener
                (
                    () => sellItem?.Invoke(_currentSelectedType)
                );

            InitItemsView(items);
        }

        private void InitItemsView(List<IMarketItem> items)
        {
            foreach (var item in items)
            {
                switch (item.Data.ResourceType)
                {
                    default:
                        break;
                    case ResourceType.Wood:
                        {
                            _wood.Init(item, OnItemClicked);
                            break;
                        }
                    case ResourceType.Iron:
                        {
                            _iron.Init(item, OnItemClicked);
                            break;
                        }
                }
            }
        }

        private void OnItemClicked(IMarketItem item)
        {
            _currentSelectedType = item.Data.ResourceType;

            switch (_currentSelectedType)
            {
                default:
                    break;
                case ResourceType.Wood:
                    {
                        _wood.SetSelected(true);
                        _iron.SetSelected(false);
                        break;
                    }
                case ResourceType.Iron:
                    {
                        _iron.SetSelected(true);
                        _wood.SetSelected(false);
                        break;
                    }
            }
            
            _exchangeAmount = item.Data.ExchangeAmount;

            _exchangeAmountText.text = _exchangeAmount.ToString();
        }

        public void Deinit()
        {
            _byItemButton.onClick.RemoveAllListeners();
            _sellItemButton.onClick.RemoveAllListeners();
            _wood.Deinit();
            _iron.Deinit();
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
        }
    }
}


