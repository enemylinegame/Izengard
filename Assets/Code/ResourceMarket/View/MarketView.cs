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
        [field: SerializeField] public TMP_Text ExchangeAmountText;
        [field: SerializeField] public TMP_Text StatusText;
        [field: SerializeField] public TMP_Text MarketAmountText;
        [field: SerializeField] public TMP_Text TimerText;
        [field: SerializeField] public GameObject MarketItemPrefab;

        [Space(10)]
        [Header("Buttons")]
        [field: SerializeField] public Button ByItemButton;
        [field: SerializeField] public Button SellItemButton;
        [field: SerializeField] public Button IncreaseExchangeButton;
        [field: SerializeField] public Button DecreaseExchangeButton;
        [field: SerializeField] public Button CloseMarketButton;

        [Space(10)]
        [Header("Items Tier Settings")]
        [field: SerializeField] public ItemsContainerView TierOneItems;
        [field: SerializeField] public ItemsContainerView TierTwoItems;
        [field: SerializeField] public ItemsContainerView TierThreeItems;

        [field: SerializeField] public MarketTradeInfoView MarketTradeInfo;

        
    }
}


