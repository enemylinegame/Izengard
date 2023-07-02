using System;
using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketEntryPoint : MonoBehaviour
    {
        [SerializeField] private MarketView _marketView;
        [SerializeField] private GlobalResourceList _globalResourceList;
        [SerializeField] private MarketDataConfig _marketData;

        [SerializeField] private Button _openMarketButton;

        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private int _initialWood = 100;
        [SerializeField] private int _initialIron = 100;

        private MarketController _marketController;

        private void Start()
        {
            var marketCustomer = new MarketCustomerController(_marketView.CustomerView, _globalResourceList.GlobalResourceConfigs);
            var globalResStock = new GlobalStock(_globalResourceList.GlobalResourceConfigs);
            var marketProvider = new TestMarketDataProvider(_marketData.MarketCoef, _marketData.MarketBuildings);

            _marketController = new MarketController(_marketView, _marketData, marketCustomer, globalResStock, marketProvider);

            globalResStock.AddResourceToStock(ResourceType.Gold, _initialGold);
            globalResStock.AddResourceToStock(ResourceType.Wood, _initialWood);
            globalResStock.AddResourceToStock(ResourceType.Iron, _initialIron);

            _openMarketButton.onClick.AddListener(ShowMarket);

        }

        private void ShowMarket()
        {
            _marketController.ShowView();
        }
    }
}
