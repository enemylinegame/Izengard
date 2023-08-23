using Code.Player;
using Code.UI;
using Code.UI.CenterPanel;
using Code.UI.MarketPanel;
using CombatSystem.Views;
using UnityEngine;

namespace Code.UI
{
    public class UIPanelsInitialization
    {
        public ResourcesPanelController ResourcesPanelController;
        public TileUIController TileUIController;
        public CenterPanelController CenterPanelController;
        public RightPanelController RightPanelController;
        public MarketPanelController MarketPanelController;
        public WarsView WarsView;
        
        public UIPanelsInitialization(GameConfig config, Canvas canvas, InputController inputController)
        {
            var topPanelFactory = new TopPanelFactory(config.UIElementsConfig, canvas);
            var bottomUIFactory = new TileUIFactory(config.UIElementsConfig, canvas);
            var centerPanelFactory = new CenterPanelFactory(config.UIElementsConfig, canvas);
            var RightPanelFactory = new RightPanelFactory(config.UIElementsConfig, canvas);
            var marketPanelFactory = new MarketPanelFactory(config.UIElementsConfig, canvas);


            ResourcesPanelController = new ResourcesPanelController(topPanelFactory);
            CenterPanelController = new CenterPanelController(centerPanelFactory);
            TileUIController = new TileUIController(bottomUIFactory, inputController, CenterPanelController);
            RightPanelController = new RightPanelController(RightPanelFactory);
            MarketPanelController = new MarketPanelController(marketPanelFactory);
            WarsView = new WarsView(TileUIController.WarsUIView, inputController);
        }
    }
}