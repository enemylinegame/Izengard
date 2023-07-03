using System.Collections.Generic;
using UnityEngine;

namespace ResourceMarket
{
    [CreateAssetMenu(fileName = nameof(MarketDataConfig), menuName = "Market/" + nameof(MarketDataConfig), order = 1)]
    public sealed class MarketDataConfig : ScriptableObject
    {
        [SerializeField] private List<MarketItemData> _marketItems;
        [SerializeField] private MarketTierData _marketTierData;
        
        [Space(10)]
        [SerializeField] private int _marketBuildings = 1;
        [SerializeField] private float _marketCoef = 0.05f;
        
        [Space(5)]
        [SerializeField] private float _marketRestoreValueDelay = 30f;
        
        public List<MarketItemData> MarketItemsData => _marketItems;
        public MarketTierData MarketTierData => _marketTierData;
        public int MarketBuildings => _marketBuildings;
        public float MarketCoef => _marketCoef;
        public float MarketRestoreValueDelay => _marketRestoreValueDelay;
    }

    [System.Serializable]
    public sealed class MarketTierData
    {
        [field: SerializeField] public int TierOneUnlockValue { get; private set; } = 0;
        [field: SerializeField] public int TierTwoUnlockValue { get; private set; } = 3;
        [field: SerializeField] public int TierThreeUnlockValue { get; private set; } = 6;
    }
}
