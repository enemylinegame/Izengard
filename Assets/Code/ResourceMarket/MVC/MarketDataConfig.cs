using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    [CreateAssetMenu(fileName = nameof(MarketDataConfig), menuName = "Market/" + nameof(MarketDataConfig), order = 1)]
    public sealed class MarketDataConfig : ScriptableObject
    {
        [SerializeField] private List<MarketItemData> _marketItems;

        public List<MarketItemData> MarketItemsData => _marketItems;
    }

    [System.Serializable]
    public struct MarketItemData
    {

        public ResourceType ResourceType;
        public int ExchangeAmount;
        public int ExchangeRate;
        public int InitialAmount;
        public int MinAmount;

        public string Name;
        public string ErrorMessage;
    }
}
