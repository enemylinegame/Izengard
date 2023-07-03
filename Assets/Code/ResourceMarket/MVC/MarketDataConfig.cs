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
    public class MarketItemData : IMarketItemData
    {
        [field: SerializeField] public TierType TierType { get; private set; }

        [field: SerializeField] public ResourceType ResourceType { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public int InitialAmount { get; private set; } = 100;
        [field: SerializeField] public int ExchangeAmount { get; private set; } = 10;
        [field: SerializeField] public int ExchangeRate { get; private set; } = 100;
        [field: SerializeField] public int MinExchange { get; private set; } = 50;
        [field: SerializeField] public float ExchangeCoef { get; private set; } = 0.45f;

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string ErrorMessage { get; private set; }
    }
}
