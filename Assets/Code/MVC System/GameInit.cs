using Code.BuildingSystem;
using Code.Player;
using Code.QuickOutline.Scripts;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using Code.Units.HireDefendersSystem;
using CombatSystem;
using Controllers.BaseUnit;
using EquipmentSystem;
using ResourceSystem;
using UnityEngine;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, WorkersTeamConfig workersTeamConfig,
        RightUI rightUI, Transform btnParents, CenterUI centerUI, BottomUI bottomUI, TopResUiVew topResUiVew,
        EndGameScreen endGameScreen,
        TowerShotConfig towerShotConfig, BuyItemScreenView buyItemScreenView, HireSystemView hireSystemView,
        EquipScreenView equipScreenView, Camera camera, TileList tileList,
        GlobalResourceList globalResourceList, OutLineSettings outLineSettings)
    {
        //TODO Do not change the structure of the script
        var tiles = GetTileList.GetTiles(gameConfig);
        var outlineController = new OutlineController(outLineSettings);
        var globalResStock = new GlobalStock(globalResourceList.GlobalResourceConfigs);
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var inputController = new InputController(outlineController);
        var uiController = new UIController(rightUI, bottomUI, centerUI, inputController);

        var levelGenerator = new GeneratorLevelController(
            tiles, gameConfig, btnConroller, btnParents, uiController);
        // var buildController = new BuildGenerator(gameConfig);
        var buildingController = new BuildingController(
            uiController, globalResStock, gameConfig, levelGenerator);

        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();
        var towershotcontroller = new TowerShotController(towerShotConfig, levelGenerator, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, 
            hireSystemView, levelGenerator);
        var bulletsController = new BulletsController();
        var waveController = new WaveController(levelGenerator, uiController, btnParents, gameConfig, bulletsController);
        var endGameController = new EndGameController(endGameScreen, levelGenerator);
        
        var workersTeamComtroller = new WorkersTeamController(
            workersTeamConfig);
        var productionManager = new ProductionManager(
            globalResStock, workersTeamComtroller, workersTeamConfig);
        var tileController = new TileController(tileList, uiController, 
            buildingController, inputController, productionManager);
        var defenderController = new DefendersController(bulletsController);
        var tileResourceUIController = new TileResourceUIController(uiController, inputController, tileController);
        var hireUnitView = new HireUnitView(rightUI.HireUnits);
        var defendersAssignController = new DefendersManager(tileController, defenderController, uiController, 
            hireUnitView, gameConfig.DefendersSets);
        inputController.Add(defendersAssignController);
        
        if (!gameConfig.ChangeVariant) new ResourceGenerator(/*buildController.Buildings, */gameConfig, levelGenerator, buildingController);
        else new ResourceGenerator(/*.Buildings, */gameConfig, levelGenerator, buildingController, 2);


        controller.Add(workersTeamComtroller);
        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        // controller.Add(buildController);
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

        // var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        // controller.Add(testDummyTargetController);
    }
}