using Code.UI;
using ResourceSystem;
using ResourceSystem.SupportClases;

namespace Code.TileSystem
{
    public class RenovationOfTheCentralBuilding
    {
        private ITextVisualizationOnUI _notificationUI;
        private GlobalStock _stock;
        private RepairAndRecoberCostCenterBuilding _cost;
        private TileUIView _uiView;
        
        public RenovationOfTheCentralBuilding(ITextVisualizationOnUI notificationUI, GlobalStock stock, RepairAndRecoberCostCenterBuilding cost, TileUIView uiView)
        {
            _notificationUI = notificationUI;
            _stock = stock;
            _cost = cost;
            _uiView = uiView;
        }

        public void RepairBuilding(float MaxHealth, ref float CurrentHealth)
        {
            if(!IsResourcesEnoughRepair(_cost)) return;
            if (CurrentHealth < MaxHealth)
            {
                _cost.RepairCost.ForEach(resourcePrice => 
                    _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));
                CurrentHealth = MaxHealth;
            }
            
        }

        public void RecoveryBuilding(float MaxHealth, ref float CurrentHealth)
        {
            if(!IsResourcesEnoughRecovery(_cost)) return;
            if (CurrentHealth < MaxHealth)
            {
                _cost.RecoveryCost.ForEach(resourcePrice => 
                    _stock.GetResourceFromStock(resourcePrice.ResourceType, resourcePrice.Cost));
                CurrentHealth = MaxHealth;
            }
        }
        
        
        private bool IsResourcesEnoughRepair(RepairAndRecoberCostCenterBuilding cost)
        {
            foreach (ResourcePriceModel resourcePriceModel in cost.RepairCost)
            {
                if (!_stock.CheckResourceInStock(resourcePriceModel.ResourceType, resourcePriceModel.Cost))
                {
                    _notificationUI.BasicTemporaryUIVisualization("you do not have enough resources to repair", 1);
                    return false;
                }
            }
            return true;
        }
        private bool IsResourcesEnoughRecovery(RepairAndRecoberCostCenterBuilding cost)
        {
            foreach (ResourcePriceModel resourcePriceModel in cost.RecoveryCost)
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