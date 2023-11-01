<<<<<<< HEAD
using Code.BuildingSystem;
using Code.Game;
using Code.Player;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using Code.Units.HireDefendersSystem;
using CombatSystem;
using CombatSystem.Views;
using Controllers.BaseUnit;
using ResourceMarket;
using ResourceSystem;
=======
using Configs;
using Tools;
>>>>>>> Dev-Anton
using UnityEngine;

public class GameInit
{
<<<<<<< HEAD
    public GameInit(Controller controller, ConfigsHolder configs, AudioSource clickAudioSource, Canvas canvas)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(configs.GameConfig);

        var outlineController = new OutlineController(configs.OutLineSettings);
        var keyInputController = new KeyInputController();
        var uiPanelInitialization = new UIPanelsInitialization(configs.UIElementsConfig, canvas);
        
        var globalStock = new GlobalStock(configs.GlobalResourceData.ResourcesData, uiPanelInitialization.ResourcesPanelController);
        var gameStateManager = new GameStateManager();
        var soundPlayer = new SoundPlayer(clickAudioSource);
        var tileGenerator = new TileGenerator(tiles, configs, uiPanelInitialization.RightPanelController);
        var inputController = new InputController(outlineController, tileGenerator);
        var buildingFactory = new BuildingFactory(uiPanelInitialization.NotificationPanel, globalStock, configs, tileGenerator);

        /* Market */
        var marketController = new MarketController(uiPanelInitialization, globalStock, buildingFactory, configs.MarketDataConfig);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towerShotController = new TowerShotController(configs, buildingFactory);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(tileGenerator, uiPanelInitialization, configs, bulletsController, enemyDestroyObserver, buildingFactory);
        var endGameController = new EndGameController(gameStateManager, uiPanelInitialization.EndGameScreenPanel, buildingFactory);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiPanelInitialization.NotificationPanel, globalStock, buildingFactory, configs.GlobalTileSettings);

        var workersTeamController = new WorkersTeamController(configs.WorkersTeamConfig);
        var productionManager = new ProductionManager(globalStock, workersTeamController, configs.WorkersTeamConfig, configs.PrescriptionsStorage, uiPanelInitialization.NotificationPanel);
        
        var tileController = new TileController(uiPanelInitialization, buildingFactory, inputController, productionManager, renovationOfTheCentralBuilding, globalStock, configs.GlobalTileSettings, tileGenerator);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiPanelInitialization.TilePanelController, inputController, tileController, configs.PrefabsHolder);
        var hireUnitView = new HireUnitView(uiPanelInitialization.RightPanelController.HireUnitView);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var warsView = new WarsView(uiPanelInitialization.TilePanelController.WarsPanel, inputController);
        var defendersAssignController = new DefendersManager(tileController, defenderController, warsView, hireUnitView, configs.DefendersSet, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(uiPanelInitialization.InGameMenuPanel, gameStateManager, keyInputController, soundPlayer);
        
        new ResourceGenerator(configs,tileGenerator, buildingFactory);
        // if (!gameConfig.ChangeVariant) new ResourceGenerator(gameConfig, configs.GlobalMineralsList,levelGenerator, buildingController);
        // else new ResourceGenerator(gameConfig, levelGenerator, buildingController, 2);

        inputController.Add(defendersAssignController);
        controller.Add(timeRemaining);
        controller.Add(workersTeamController);
        controller.Add(tileGenerator);
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
        
        inputController.Add(uiPanelInitialization.RightPanelController);
        inputController.Add(uiPanelInitialization.TilePanelController);

        gameStateManager.OnDisable += controller.OnDisable;

        var initialResources = configs.GlobalResourceData.InitialResourceData.InitialResources;
        foreach (var initResData in initialResources)
        {
            globalStock.AddResourceToStock(initResData.ResourceType, initResData.Amount);
        }

        // var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        // controller.Add(testDummyTargetController);
=======
    public GameInit(
        Controller controller, 
        ConfigsHolder configs, 
        AudioSource clickAudioSource, 
        Canvas canvas)
    {
        var timeRemainingService = new TimeRemainingController();
            
        controller.Add(timeRemainingService);
>>>>>>> Dev-Anton
    }
}