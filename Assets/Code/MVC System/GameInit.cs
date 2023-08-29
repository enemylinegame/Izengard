using Code.BuildingSystem;
using Code.Game;
using Code.Player;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using Code.Units.HireDefendersSystem;
using CombatSystem;
using Controllers.BaseUnit;
using ResourceMarket;
using ResourceSystem;
using UnityEngine;
using Wave;

public class GameInit
{
    public GameInit(Controller controller, ConfigsHolder configs, AudioSource clickAudioSource, Canvas canvas)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(configs.GameConfig);

        var outlineController = new OutlineController(configs.OutLineSettings);
        var inputController = new InputController(outlineController);
        var keyInputController = new KeyInputController();
        var uiPanelInitialization = new UIPanelsInitialization(configs.UIElementsConfig, canvas, inputController);
        
        var globalStock = new GlobalStock(configs.GlobalResourceData, uiPanelInitialization.ResourcesPanelController);
        var gameStateManager = new GameStateManager();
        var soundPlayer = new SoundPlayer(clickAudioSource);
        var levelGenerator = new GeneratorLevelController(tiles, configs, uiPanelInitialization.RightPanelController);
        var buildingFactory = new BuildingFactory(uiPanelInitialization.NotificationPanel, globalStock, configs, levelGenerator);

        /* Market */
        var marketController = new MarketController(uiPanelInitialization, globalStock, buildingFactory, configs.MarketDataConfig);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towerShotController = new TowerShotController(configs, buildingFactory);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiPanelInitialization, configs, bulletsController, enemyDestroyObserver, buildingFactory);
        var endGameController = new EndGameController(gameStateManager, uiPanelInitialization.EndGameScreenPanel, buildingFactory);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiPanelInitialization.NotificationPanel, globalStock, levelGenerator, buildingFactory, configs.GlobalTileSettings);

        var workersTeamController = new WorkersTeamController(configs.WorkersTeamConfig);
        var productionManager = new ProductionManager(globalStock, workersTeamController, configs.WorkersTeamConfig, configs.PrescriptionsStorage, uiPanelInitialization.NotificationPanel);
        
        var tileController = new TileController(uiPanelInitialization, buildingFactory, inputController, productionManager, renovationOfTheCentralBuilding, globalStock, configs.GlobalTileSettings);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiPanelInitialization.TilePanelController, inputController, tileController, configs.PrefabsHolder);
        var hireUnitView = new HireUnitView(uiPanelInitialization.RightPanelController.HireUnitView);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiPanelInitialization.WarsView, hireUnitView, configs.DefendersSet, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(uiPanelInitialization.InGameMenuPanel, gameStateManager, keyInputController, soundPlayer);
        
        new ResourceGenerator(configs,levelGenerator, buildingFactory);
        // if (!gameConfig.ChangeVariant) new ResourceGenerator(gameConfig, configs.GlobalMineralsList,levelGenerator, buildingController);
        // else new ResourceGenerator(gameConfig, levelGenerator, buildingController, 2);

        inputController.Add(defendersAssignController);
        controller.Add(timeRemaining);
        controller.Add(workersTeamController);
        controller.Add(levelGenerator);
        controller.Add(buildingFactory);
        controller.Add(unitController);
        controller.Add(inputController);
        controller.Add(waveController);
        controller.Add(endGameController);
        controller.Add(towerShotController);
        controller.Add(tileController);
        controller.Add(defenderController);
        controller.Add(bulletsController);
        controller.Add(marketController);
        controller.Add(hireDefendersManager);
        controller.Add(keyInputController);

        gameStateManager.OnDisable += controller.OnDisable;
    }
}