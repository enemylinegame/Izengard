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
using EquipmentSystem;
using ResourceMarket;
using ResourceSystem;
using UnityEngine;
using Wave;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, WorkersTeamConfig workersTeamConfig,
        RightUI rightUI, Transform btnParents, CenterUI centerUI, BottomUI bottomUI, TopResUiVew topResUiVew,
        EndGameScreen endGameScreen, TowerShotConfig towerShotConfig, BuyItemScreenView buyItemScreenView, 
        HireSystemView hireSystemView, EquipScreenView equipScreenView, Camera camera, GlobalTileSettings globalTileSettings,
        GlobalResourceData globalResourceList, OutLineSettings outLineSettings, MarketDataConfig marketData, 
        MarketView marketView, 
        InGameMenuUI inGameMenuUI, AudioSource clickAudioSource)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(gameConfig);

        var outlineController = new OutlineController(outLineSettings);
        var globalResStock = new GlobalStock(globalResourceList, topResUiVew);
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var gameStateManager = new GameStateManager();
        var inputController = new InputController(outlineController);
        var keyInputController = new KeyInputController();
        var soundPlayer = new SoundPlayer(clickAudioSource);
        var uiController = new UIController(rightUI, bottomUI, centerUI, inputController, marketView);

        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, btnConroller, btnParents, uiController, globalTileSettings);
        // var buildController = new BuildGenerator(gameConfig);
        var buildingController = new BuildingFactory(uiController, globalResStock, gameConfig, levelGenerator, globalTileSettings);

        /* Market */
        var marketController = new MarketController(uiController, globalResStock, buildingController, marketData);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalResStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        TimersHolder.SetTimeRemainingController(timeRemaining);
        var towershotcontroller = new TowerShotController(towerShotConfig, buildingController, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, hireSystemView, levelGenerator);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiController, btnParents, gameConfig, bulletsController, enemyDestroyObserver, buildingController);
        var endGameController = new EndGameController(gameStateManager, endGameScreen, buildingController);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiController.CenterUI.BaseNotificationUI, globalResStock, uiController.BottomUI.TileUIView, levelGenerator, buildingController, globalTileSettings);

        var workersTeamComtroller = new WorkersTeamController(workersTeamConfig);
        var productionManager = new ProductionManager(globalResStock, workersTeamComtroller, workersTeamConfig, gameConfig.PrescriptionsStorage, uiController.CenterUI.BaseNotificationUI);
        
        var tileController = new TileController(uiController, buildingController, inputController, productionManager, renovationOfTheCentralBuilding, globalResStock, globalTileSettings);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiController, inputController, tileController, gameConfig);
        var hireUnitView = new HireUnitView(rightUI.HireUnits);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalResStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiController, hireUnitView, gameConfig.DefendersSets, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(inGameMenuUI, gameStateManager, keyInputController, soundPlayer);

        if (!gameConfig.ChangeVariant) new ResourceGenerator(/*buildController.Buildings, */gameConfig, levelGenerator, buildingController);
        else new ResourceGenerator(/*.Buildings, */gameConfig, levelGenerator, buildingController, 2);

        inputController.Add(defendersAssignController);
        controller.Add(timeRemaining);
        controller.Add(uiController);
        controller.Add(workersTeamComtroller);
        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildingController);
        controller.Add(unitController);
        controller.Add(inputController);
        controller.Add(waveController);
        controller.Add(endGameController);
        controller.Add(towershotcontroller);
        controller.Add(hireSystemController);
        controller.Add(tileController);
        controller.Add(defenderController);
        controller.Add(bulletsController);
        controller.Add(marketController);
        controller.Add(hireDefendersManager);
        controller.Add(keyInputController);

        gameStateManager.OnDisable += controller.OnDisable;

        // var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        // controller.Add(testDummyTargetController);
    }
}