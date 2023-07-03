using UnityEngine;

namespace ResourceMarket
{
    public sealed class StubItemModel : BaseItemModel
    {
        public StubItemModel(IMarketItemData data) : base(data)
        {
            Debug.LogWarning($"{nameof(StubItemModel)} was created");
        }

        public override int BuyCost => 0;

        public override int ExchangeCost => 0;

        public override void DecreaseAmount(int amount) { }

        public override void IncreaseAmount(int amount) { }

        public override void RestoreValue() { }
    }
}
