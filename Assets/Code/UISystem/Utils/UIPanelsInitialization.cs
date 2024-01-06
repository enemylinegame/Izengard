using UnityEngine;

namespace UI
{
    public class UIPanelsInitialization
    {
        public readonly BuildingPanelController BuildingPanelController;
        public readonly FunctionPanelController FunctionPanelController;
        public readonly BuildingInfoPanelController BuildingInfoPanelController;
        public readonly InfoPanelController infoPanelController;
        public readonly ResPanelController ResPanelController;
        
        public UIPanelsInitialization(UIElementsConfig config, Canvas canvas)
        {
            var infoPanelFactory = new InfoPanelFactory(config, canvas);
            var buildingPanelFactory = new BuildingPanelFactory(config, canvas);
            var functionPanelFactory = new FunctionPanelFactory(config, canvas);
            var buildingInfoPanelFactory = new BuildingInfoPanelFactory(config, canvas);
            //var resPanelFactory = new ResPanelFactory(config, canvas);

            infoPanelController = new InfoPanelController(infoPanelFactory);
            BuildingPanelController = new BuildingPanelController(buildingPanelFactory);
            FunctionPanelController = new FunctionPanelController(functionPanelFactory);
            BuildingInfoPanelController = new BuildingInfoPanelController(buildingInfoPanelFactory);
            //ResPanelController = new ResPanelController(resPanelFactory);
        }
    }
}