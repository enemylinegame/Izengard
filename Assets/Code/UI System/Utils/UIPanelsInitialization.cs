using UnityEngine;

namespace Code.UI
{
    public class UIPanelsInitialization
    {
        public readonly BuildingPanelController BuildingPanelController;
        public readonly FunctionPanelController FunctionPanelController;
        public readonly InfoPanelController infoPanelController;
        public readonly ResPanelController ResPanelController;
        
        public UIPanelsInitialization(UIElementsConfig config, Canvas canvas)
        {
            var infoPanelFactory = new InfoPanelFactory(config, canvas);
            var buildingPanelFactory = new BuildingPanelFactory(config, canvas);
            var functionPanelFactory = new FunctionPanelFactory(config, canvas);
            var resPanelFactory = new ResPanelFactory(config, canvas);

            infoPanelController = new InfoPanelController(infoPanelFactory);
            BuildingPanelController = new BuildingPanelController(buildingPanelFactory);
            FunctionPanelController = new FunctionPanelController(functionPanelFactory);
            ResPanelController = new ResPanelController(resPanelFactory);
        }
    }
}