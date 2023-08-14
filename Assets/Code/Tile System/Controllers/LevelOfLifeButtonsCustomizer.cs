using Code.BuildingSystem;
using Code.UI;
using ResourceSystem;
using ResourceSystem.SupportClases;

namespace Code.TileSystem
{
    public class LevelOfLifeButtonsCustomizer
    {
        private ITextVisualizationOnUI _notificationUI;
        private GlobalStock _stock;
        private TileUIView _uiView;
        private readonly GeneratorLevelController _levelController;
        private readonly BuildingFactory _buildingFactory;
        private readonly GlobalTileSettings _tileSettings;

        public LevelOfLifeButtonsCustomizer(ITextVisualizationOnUI notificationUI, GlobalStock stock, TileUIView uiView,
            GeneratorLevelController levelController, BuildingFactory buildingFactory, GlobalTileSettings tileSettings)
        {
            _notificationUI = notificationUI;
            _stock = stock;
            _uiView = uiView;
            _levelController = levelController;
            _buildingFactory = buildingFactory;
            _tileSettings = tileSettings;
        }

        public void RepairBuilding(TileModel model)
        {
            if(!IsResourcesEnoughRepair(model.TileConfig)) return;
            if (model.CenterBuilding.CurrentHealth < model.CenterBuilding.MaxHealth)
            {
                model.TileConfig.PriceRepair.ForEach(resourcePrice => 
                    _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));
                model.CenterBuilding.CurrentHealth = model.CenterBuilding.MaxHealth;
            }
            
        }

        public void RecoveryBuilding(TileModel model)
        {
            if(!IsResourcesEnoughRecovery(model.TileConfig)) return;
            if (model.CenterBuilding.CurrentHealth < model.CenterBuilding.MaxHealth)
            {
                model.TileConfig.PriceRecovery.ForEach(resourcePrice =>
                {
                    _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost);
                });
                _buildingFactory.DummyController.Spawn(_tileSettings.MaxHealthCenterBuilding);
            }
        }
        
        
        
        
        private bool IsResourcesEnoughRepair(TileConfig cost)
        {
            foreach (ResourcePriceModel resourcePriceModel in cost.PriceRepair)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationUI.BasicTemporaryUIVisualization("you do not have enough resources to repair", 1);
                    return false;
                }
            }
            return true;
        }
        private bool IsResourcesEnoughRecovery(TileConfig cost)
        {
            foreach (ResourcePriceModel resourcePriceModel in cost.PriceRecovery)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationUI.BasicTemporaryUIVisualization("you do not have enough resources to recovery", 1);
                    return false;
                }
            }
            return true;
        }
    }
}