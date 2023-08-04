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
        HireSystemView hireSystemView, EquipScreenView equipScreenView, Camera camera, TileList tileList,
        GlobalResourceData globalResourceList, OutLineSettings outLineSettings, MarketDataConfig marketData, 
        MarketView marketView, RepairAndRecoberCostCenterBuilding repairAndRecoberCostCenterBuilding, 
        InGameMenuUI inGameMenuUI)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(gameConfig);

        var outlineController = new OutlineController(outLineSettings);
        var globalResStock = new GlobalStock(globalResourceList, topResUiVew);
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var pauseManager = new PauseManager();
        var inputController = new InputController(outlineController);
        var keyInputController = new KeyInputController();
        var uiController = new UIController(rightUI, bottomUI, centerUI, inputController, marketView);

        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, btnConroller, btnParents, uiController);
        // var buildController = new BuildGenerator(gameConfig);
        var buildingController = new BuildingFactory(uiController, globalResStock, gameConfig, levelGenerator);

        /* Market */
        var marketController = new MarketController(uiController, globalResStock, buildingController, marketData);

        var enemyDestroyObserver = new EnemyDestroyObserver(globalResStock);
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towershotcontroller = new TowerShotController(towerShotConfig, buildingController, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, hireSystemView, levelGenerator);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiController, btnParents, gameConfig, bulletsController, enemyDestroyObserver, buildingController);
        var endGameController = new EndGameController(endGameScreen, levelGenerator);
        var renovationOfTheCentralBuilding = new LevelOfLifeButtonsCustomizer(uiController.CenterUI.BaseNotificationUI, globalResStock, uiController.BottomUI.TileUIView, levelGenerator, buildingController);

        var workersTeamComtroller = new WorkersTeamController(workersTeamConfig);
        var productionManager = new ProductionManager(globalResStock, workersTeamComtroller, workersTeamConfig, gameConfig.PrescriptionsStorage, uiController.CenterUI.BaseNotificationUI);
        
        var tileController = new TileController(tileList, uiController, buildingController, inputController, productionManager, renovationOfTheCentralBuilding, globalResStock);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiController, inputController, tileController, gameConfig);
        var hireUnitView = new HireUnitView(rightUI.HireUnits);
        var paymentDefendersSystem = new PaymentDefendersSystem(globalResStock);
        var hireDefendersManager = new HireDefenderProgressManager();
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiController, hireUnitView, gameConfig.DefendersSets, paymentDefendersSystem, hireDefendersManager);
        inputController.Add(defendersAssignController);
        var inGameMenuController = new InGameMenuController(inGameMenuUI, pauseManager, keyInputController);

        if (!gameConfig.ChangeVariant) new ResourceGenerator(/*buildController.Buildings, */gameConfig, levelGenerator, buildingController);
        else new ResourceGenerator(/*.Buildings, */gameConfig, levelGenerator, buildingController, 2);

        inputController.Add(defendersAssignController);
        controller.Add(workersTeamComtroller);
        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildingController);
        controller.Add(timeRemaining);
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

        // var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        // controller.Add(testDummyTargetController);
    }
}