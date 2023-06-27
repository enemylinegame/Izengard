using System;
using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketItemView :  MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private Button _byItemButton;
        [SerializeField] private Button _sellItemButton;

        private Action<ResourceType> _onByItem;
        private Action<ResourceType> _onSellItem;

        public void Init(Action<ResourceType> byItemAction, Action<ResourceType> sellItemAction)
        {
            _onByItem = byItemAction;
            _onSellItem = sellItemAction;

            _byItemButton.onClick.AddListener(ByItem);
            _sellItemButton.onClick.AddListener(SellItem);
        }

        public void Deinit()
        {
            _onByItem = default;
            _onSellItem = default;

            _byItemButton.onClick.RemoveListener(ByItem);
            _sellItemButton.onClick.RemoveListener(SellItem);
        }

        private void ByItem()
        {
            _onByItem?.Invoke(_resourceType);
        }

        private void SellItem()
        {
            _onSellItem?.Invoke(_resourceType);
        }

        private void OnDestroy()
        {
            Deinit();
        }

    }
}
