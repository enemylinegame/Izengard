using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceMarket
{
    public sealed class MarketEntryPoint : MonoBehaviour
    {
        [SerializeField] private MarketView _marketView;
        [SerializeField] private TopResUiVew _topResUiVew;
        [SerializeField] private CustomerDataView _customerView;

        [SerializeField] private GlobalResourceList _globalResourceList;
        [SerializeField] private MarketDataConfig _marketData;

        [SerializeField] private Button _openMarketButton;

        [SerializeField] private int _initialGold = 1000;
        [SerializeField] private int _initialWood = 100;
        [SerializeField] private int _initialIron = 100;

        private MarketController _marketController;
        private CustomerController _customerController;

        private void Start()
        {
            var globalResStock = new GlobalStock(_globalResourceList.GlobalResourceConfigs, _topResUiVew);
            _marketController = new MarketController(_marketView, _marketData, globalResStock, null, null);    
            _customerController = new CustomerController(_customerView, _globalResourceList.GlobalResourceConfigs, globalResStock);

            globalResStock.AddResourceToStock(ResourceType.Gold, _initialGold);
            globalResStock.AddResourceToStock(ResourceType.Wood, _initialWood);
            globalResStock.AddResourceToStock(ResourceType.Iron, _initialIron);

            _openMarketButton.onClick.AddListener(ShowMarket);
        }

        private void ShowMarket()
        {
            _marketController.ShowView();
        }
        private void Update()
        {
            _marketController.OnUpdate(Time.deltaTime);
        }

    }
}
