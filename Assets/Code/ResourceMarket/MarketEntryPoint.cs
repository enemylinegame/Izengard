using ResourceSystem;
using UnityEngine;

namespace ResourceMarket
{
    public sealed class MarketEntryPoint : MonoBehaviour
    {
        [SerializeField] private MarketView _marketView;
        [SerializeField] private GlobalResourceList _globalResourceList;
        [SerializeField] private MarketDataConfig _marketData;
        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private int _initialWood = 100;
        [SerializeField] private int _initialIron = 100;

        private MarketController _marketController;

        private void Start()
        {
            var globalResStock = new GlobalStock(_globalResourceList.GlobalResourceConfigs);
            var marketProvider = new TestMarketDataProvider(_marketData.MarketCoef, _marketData.MarketBuildings);
            _marketController = new MarketController(_marketView, _marketData, globalResStock, marketProvider);

            globalResStock.AddResourceToStock(ResourceType.Gold, _initialGold);
            globalResStock.AddResourceToStock(ResourceType.Wood, _initialWood);
            globalResStock.AddResourceToStock(ResourceType.Iron, _initialIron);

            globalResStock.ResourceValueChanged += OnWoodChange;
            globalResStock.ResourceValueChanged += OnIronChange;

        }

        private void OnWoodChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Wood) 
                return;

            Debug.Log($"Wood in stock: {value}");
        }


        private void OnIronChange(ResourceType resourceType, int value)
        {
            if (resourceType != ResourceType.Iron)
                return;

            Debug.Log($"Iron in stock: {value}");
        }

    }
}
