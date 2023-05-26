
using Code;
using Code.BuildingSystem;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using CombatSystem;
using Controllers;
using Controllers.BaseUnit;
using EquipmentSystem;
using ResourceSystem;
using UnityEngine;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, WorkersTeamConfig workersTeamConfig,
        RightUI rightUI, Transform btnParents, CenterUI centerUI, BottonUI bottonUI, EndGameScreen endGameScreen, 
        TowerShotConfig towerShotConfig, BuyItemScreenView buyItemScreenView, HireSystemView hireSystemView , 
        EquipScreenView equipScreenView, Camera camera, TileList tileList,
        GlobalResourceList globalResourceList)
    {

        var tiles = GetTileList.GetTiles(gameConfig);
        
        var globalResStock = new GlobalStock(globalResourceList.GlobalResourceConfigs);
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        
        var levelGenerator = new GeneratorLevelController(
            tiles, gameConfig, rightUI, btnConroller, btnParents, bottonUI);

        var buildController = new BuildGenerator(gameConfig);
        var inputController = new InputController();
        var uiController = new UIController(
            rightUI, bottonUI, centerUI, inputController);

        var buildingController = new BuildingController(
            uiController, globalResStock);
        
        if (!gameConfig.ChangeVariant)
        {
            new ResourceGenerator(buildController.Buildings, 
                gameConfig, levelGenerator, buildingController);
        }
        else
        {
            new ResourceGenerator(buildController.Buildings, 
                gameConfig, levelGenerator, buildingController, 2);
        }
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();

        var towershotcontroller = new TowerShotController(
            towerShotConfig, levelGenerator, gameConfig.Bullet);

        var eqScreenController = new EquipScreenController(
            equipScreenView, camera);
        
        var hireSystemController = new HireSystemController(
            globalResStock, buyItemScreenView, 
            eqScreenController, hireSystemView, levelGenerator);

        var waveController = new WaveController(levelGenerator, uiController, 
            btnParents, gameConfig);

        var endGameController = new EndGameController(
            endGameScreen, levelGenerator);
        
        var workersTeamComtroller = new WorkersTeamController(workersTeamConfig);

        var tileController = new TileController(tileList, uiController, 
            buildingController, inputController, workersTeamComtroller);

        var tileResourceUIController = new TileResourceUIController(
            uiController, inputController, tileController);

        var defenderController = new DefendersController(
            tileController,uiController, gameConfig.Defender);

        var defendersAssignController = new DefendersManager(
            tileController, defenderController, uiController);


        controller.Add(workersTeamComtroller);
        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildController);
        controller.Add(timeRemaining);
        controller.Add(unitController);
        controller.Add(inputController);
        controller.Add(waveController);
        controller.Add(endGameController);
        controller.Add(towershotcontroller);
        controller.Add(hireSystemController);
        controller.Add(tileController);
        controller.Add(defenderController);

        var testDummyTargetController = new TestDummyTargetController(
            levelGenerator, gameConfig.TestBuilding);
        controller.Add(testDummyTargetController);
    }
}