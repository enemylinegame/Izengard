using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    [CreateAssetMenu(fileName = nameof(MarketDataConfig), menuName = "Market/" + nameof(MarketDataConfig), order = 1)]
    public sealed class MarketDataConfig : ScriptableObject
    {
        [SerializeField] private List<MarketItemData> _marketItems;
        [SerializeField] private int _marketBuildings = 1;
        [SerializeField] private double _marketCoef = 0.05;

        public List<MarketItemData> MarketItemsData => _marketItems;
        public int MarketBuildings => _marketBuildings;
        public double MarketCoef => _marketCoef;
    }

    [System.Serializable]
    public struct MarketItemData
    {

        public ResourceType ResourceType;
        public int ExchangeAmount;
        public int ExchangeRate;
        public int MinExchange;
        public double ExchangeCoef;
        public int InitialAmount;

        public string Name;
        public string ErrorMessage;
    }
}
