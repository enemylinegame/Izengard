
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
    public GameInit(Controller controller, GameConfig gameConfig, RightUI rightUI, 
        Transform btnParents, CenterUI centerUI, BottonUI bottonUI, EndGameScreen endGameScreen, 
        TowerShotConfig towerShotConfig, BuyItemScreenView buyItemScreenView, HireSystemView hireSystemView , 
        EquipScreenView equipScreenView, Camera camera, BaseCenterText centerText, TileUIView tileUIView, TileList tileList,
        GlobalResourceList globalResourceList)
    {

        var tiles = GetTileList.GetTiles(gameConfig);
        
        var globalResStock = new GlobalStock(globalResourceList.GlobalResourceConfigs);
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, rightUI, btnConroller, btnParents, bottonUI);
        var buildController = new BuildGenerator(gameConfig);
        var buildingController = new BuildingController(centerText);
        
        if (!gameConfig.ChangeVariant)
        {
            new ResourceGenerator(buildController.Buildings, gameConfig, levelGenerator, buildingController);
        }
        else
        {
            new ResourceGenerator(buildController.Buildings, gameConfig, levelGenerator, 2);
        }
        var unitController = new UnitController();
        var timeRemaining = new TimeRemainingController();

        var uiController = new UIController(rightUI, bottonUI, centerUI);
        var towershotcontroller = new TowerShotController(towerShotConfig, levelGenerator, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, hireSystemView, levelGenerator);
        var waveController = new WaveController(levelGenerator, rightUI, btnParents, gameConfig, centerText);
        var endGameController = new EndGameController(endGameScreen, levelGenerator);
        var tilecontroller = new TileController(tileList, tileUIView, centerText, uiController, buildController, globalResStock, buildingController);
        var inputController = new InputController(tilecontroller, uiController);



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
        controller.Add(tilecontroller);

        var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        controller.Add(testDummyTargetController);
    }
}