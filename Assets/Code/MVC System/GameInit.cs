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
    public GameInit(Controller controller, GameConfig gameConfig, WorkersTeamConfig workersTeamConfig,Transform btnParents,
        EndGameScreenPanel endGameScreenPanel, TowerShotConfig towerShotConfig, GlobalTileSettings globalTileSettings,
        GlobalResourceData globalResourceList, OutLineSettings outLineSettings, MarketDataConfig marketData, 
        InGameMenuPanel inGameMenuPanel, AudioSource clickAudioSource, Canvas canvas)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(gameConfig);

        var outlineController = new OutlineController(outLineSettings);
        var inputController = new InputController(outlineController);
        var uiPanelInitialization = new UIPanelsInitialization(gameConfig, canvas, inputController);
        var globalResStock = new GlobalStock(globalResourceList, uiPanelInitialization.ResourcesPanelController);
        var btnConroller = new BtnUIController(uiPanelInitialization.RightPanelController, gameConfig);
        var gameStateManager = new GameStateManager();
        var keyInputController = new KeyInputController();
        var soundPlayer = new SoundPlayer(clickAudioSource);

        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, btnConroller, btnParents, uiPanelInitialization.RightPanelController, globalTileSettings);
        var buildingController = new BuildingFactory(uiPanelInitialization.NotificationPanel, globalResStock, gameConfig, levelGenerator, globalTileSettings);

        /* Market */
        var marketController = new MarketController(uiPanelInitialization, globalResStock, buildingController, marketData);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalResStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towerShotController = new TowerShotController(towerShotConfig, buildingController, gameConfig.Bullet);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiPanelInitialization, btnParents, gameConfig, bulletsController, enemyDestroyObserver, buildingController);
        var endGameController = new EndGameController(gameStateManager, uiPanelInitialization.EndGameScreenPanel, buildingController);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiPanelInitialization.NotificationPanel, globalResStock, levelGenerator, buildingController, globalTileSettings);

        var workersTeamController = new WorkersTeamController(workersTeamConfig);
        var productionManager = new ProductionManager(globalResStock, workersTeamController, workersTeamConfig, gameConfig.PrescriptionsStorage, uiPanelInitialization.NotificationPanel);
        
        var tileController = new TileController(uiPanelInitialization, buildingController, inputController, productionManager, renovationOfTheCentralBuilding, globalResStock, globalTileSettings);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiPanelInitialization.TilePanelController, inputController, tileController, gameConfig);
        var hireUnitView = new HireUnitView(uiPanelInitialization.RightPanelController.HireUnitView);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalResStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiPanelInitialization.WarsView, hireUnitView, gameConfig.DefendersSets, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(uiPanelInitialization.InGameMenuPanel, gameStateManager, keyInputController, soundPlayer);

        if (!gameConfig.ChangeVariant) new ResourceGenerator(gameConfig, levelGenerator, buildingController);
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