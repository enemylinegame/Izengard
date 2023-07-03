using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    public interface IMarketItemData
    {
        public ResourceType ResourceType { get; }
        public Sprite Icon { get; } 
        public int ExchangeAmount { get; }
        public int ExchangeRate { get; }
        public int MinExchange { get; }
        public float ExchangeCoef { get; }
        public int InitialAmount { get; }

        public string Name { get; }
        public string ErrorMessage { get; }
    }
}
