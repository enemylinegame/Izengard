using Code.BuildingSystem;
using Code.Game;
using Code.Player;
using Code.QuickOutline.Scripts;
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
    public GameInit(Controller controller, GameConfig gameConfig, ConfigsHolder configs, PrefabsHolder prefabs,Transform btnParents, 
        AudioSource clickAudioSource, Canvas canvas)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(gameConfig);

        var outlineController = new OutlineController(configs.OutLineSettings);
        var inputController = new InputController(outlineController);
        var uiPanelInitialization = new UIPanelsInitialization(configs.UIElementsConfig, canvas, inputController);
        var globalResStock = new GlobalStock(configs.GlobalResourceData, uiPanelInitialization.ResourcesPanelController);
        var btnConroller = new BtnUIController(uiPanelInitialization.RightPanelController, gameConfig);
        var gameStateManager = new GameStateManager();
        var keyInputController = new KeyInputController();
        var soundPlayer = new SoundPlayer(clickAudioSource);

        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, btnConroller, btnParents, uiPanelInitialization.RightPanelController, configs.GlobalTileSettings);
        var buildingController = new BuildingFactory(uiPanelInitialization.NotificationPanel, globalResStock, prefabs, configs, levelGenerator, configs.GlobalTileSettings);

        /* Market */
        var marketController = new MarketController(uiPanelInitialization, globalResStock, buildingController, configs.MarketDataConfig);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalResStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towerShotController = new TowerShotController(configs.TowerShotConfig, buildingController, prefabs.Bullet);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiPanelInitialization, btnParents, configs, bulletsController, enemyDestroyObserver, buildingController);
        var endGameController = new EndGameController(gameStateManager, uiPanelInitialization.EndGameScreenPanel, buildingController);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiPanelInitialization.NotificationPanel, globalResStock, levelGenerator, buildingController, configs.GlobalTileSettings);

        var workersTeamController = new WorkersTeamController(configs.WorkersTeamConfig);
        var productionManager = new ProductionManager(globalResStock, workersTeamController, configs.WorkersTeamConfig, configs.PrescriptionsStorage, uiPanelInitialization.NotificationPanel);
        
        var tileController = new TileController(uiPanelInitialization, buildingController, inputController, productionManager, renovationOfTheCentralBuilding, globalResStock, configs.GlobalTileSettings);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiPanelInitialization.TilePanelController, inputController, tileController, prefabs);
        var hireUnitView = new HireUnitView(uiPanelInitialization.RightPanelController.HireUnitView);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalResStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiPanelInitialization.WarsView, hireUnitView, configs.DefendersSet, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(uiPanelInitialization.InGameMenuPanel, gameStateManager, keyInputController, soundPlayer);

        if (!gameConfig.ChangeVariant) new ResourceGenerator(gameConfig, configs.GlobalMineralsList,levelGenerator, buildingController);
        else new ResourceGenerator(gameConfig, levelGenerator, buildingController, 2);

        inputController.Add(defendersAssignController);
        controller.Add(timeRemaining);
        controller.Add(workersTeamController);
        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildingController);
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