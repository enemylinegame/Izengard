using System;
using UnityEngine;

namespace ResourceMarket
{
    public class MarketDataProvider : IMarketDataProvider
    {
        private float _marketCoef;
        private int _marketAmount;

        public event Action<int> OnMarketAmountChange;

        public float MarketCoef => _marketCoef;

        public int MarketAmount 
        {
            get => _marketAmount;
            private set
            {
                if(_marketAmount != value) 
                {
                    _marketAmount = Mathf.Clamp(value, 0, int.MaxValue);
                    OnMarketAmountChange?.Invoke(_marketAmount);
                }
            }
        }

        public MarketDataProvider(float marketCoef)
        {
            _marketCoef = marketCoef;
        }

        public void AddMarket() => MarketAmount++;

        public void RemoveMarket() => MarketAmount--;
    }
}
