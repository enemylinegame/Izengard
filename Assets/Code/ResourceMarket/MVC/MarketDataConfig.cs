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
        [SerializeField] private float _marketCoef = 0.05f;

        public List<MarketItemData> MarketItemsData => _marketItems;
        public int MarketBuildings => _marketBuildings;
        public float MarketCoef => _marketCoef;
    }

    [System.Serializable]
    public struct MarketItemData
    {

        public ResourceType ResourceType;
        public int ExchangeAmount;
        public int ExchangeRate;
        public int MinExchange;
        public float ExchangeCoef;
        public int InitialAmount;

        public string Name;
        public string ErrorMessage;
    }
}
