using UnityEngine;

namespace ResourceMarket
{
    [System.Serializable]
    public sealed class MarketTierData
    {
        [field: SerializeField] public int TierOneUnlockValue { get; private set; } = 0;
        [field: SerializeField] public int TierTwoUnlockValue { get; private set; } = 3;
        [field: SerializeField] public int TierThreeUnlockValue { get; private set; } = 6;
    }
}
