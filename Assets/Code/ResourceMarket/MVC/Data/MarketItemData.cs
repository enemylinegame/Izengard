using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    [System.Serializable]
    public class MarketItemData : IMarketItemData
    {
        [field: SerializeField] public TierType TierType { get; private set; }

        [field: SerializeField] public ResourceType ResourceType { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public int InitialAmount { get; private set; } = 100;
        [field: SerializeField] public int ExchangeAmount { get; private set; } = 10;
        [field: SerializeField] public int ExchangeRate { get; private set; } = 100;
        [field: SerializeField] public float ExchangeCoef { get; private set; } = 0.45f;

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string ErrorMessage { get; private set; }
    }

    
}
