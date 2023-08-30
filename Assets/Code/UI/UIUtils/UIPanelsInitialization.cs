using Code.Player;
using CombatSystem.Views;
using Core;
using UnityEngine;

namespace Code.UI
{
    public class UIPanelsInitialization
    {
        public readonly ResourcesPanelController ResourcesPanelController;
        public readonly TilePanelController TilePanelController;
        public readonly CenterPanelController CenterPanelController;
        public readonly RightPanelController RightPanelController;
        public readonly MarketPanelController MarketPanelController;
        public readonly TileBuildingBoardController TileMenu;
        public readonly TileMainBoardController TileMainBoard;
        public readonly TileResourcesPanelController TileResourcesPanel;
        public readonly NotificationPanelController NotificationPanel;
        public readonly EndGameScreenPanelController EndGameScreenPanel;
        public readonly InGameMenuPanelController InGameMenuPanel;
        public readonly WarsView WarsView;
        
        public UIPanelsInitialization(UIElementsConfig config, Canvas canvas, InputController inputController)
        {
            var topPanelFactory = new ResourcesPanelFactory(config, canvas);
            var bottomUIFactory = new TilePanelFactory(config, canvas);
            var centerPanelFactory = new CenterPanelFactory(config, canvas);
            var rightPanelFactory = new RightPanelFactory(config, canvas);
            var marketPanelFactory = new MarketPanelFactory(config, canvas);
            var endGameScreenPanelFactory = new EndGameScreenPanelFactory(config, canvas);
            var inGameMenuPanelFactory = new InGameMenuPanelFactory(config, canvas);

            ResourcesPanelController = new ResourcesPanelController(topPanelFactory);
            CenterPanelController = new CenterPanelController(centerPanelFactory);
            TilePanelController = new TilePanelController(bottomUIFactory, inputController, CenterPanelController);
            RightPanelController = new RightPanelController(rightPanelFactory, inputController);
            MarketPanelController = new MarketPanelController(marketPanelFactory);
            EndGameScreenPanel = new EndGameScreenPanelController(endGameScreenPanelFactory);
            InGameMenuPanel = new InGameMenuPanelController(inGameMenuPanelFactory);
            WarsView = new WarsView(TilePanelController.WarsPanel, inputController);

            TileMenu = TilePanelController.TileMenu;
            TileMainBoard = TilePanelController.TileMainBoard;
            TileResourcesPanel = TilePanelController.TileResourcesPanel;
            NotificationPanel = CenterPanelController.NotificationPanel;
        }
    }
}