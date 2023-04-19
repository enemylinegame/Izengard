using Controllers;
using Controllers.BaseUnit;
using Controllers.OutPost;
using UnityEngine;
using ResourceSystem;
using BuildingSystem;
using Code;
using Code.BuildingSystem;
using Code.TileSystem;
using Code.TowerShot;
using Code.UI;
using Views.BaseUnit.UI;
using CombatSystem;
using EquipmentSystem;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, RightUI rightUI, 
        Transform btnParents, LeftUI leftUI, CenterUI centerUI,BottonUI bottonUI,
        BuildingsUI buildingsUI, GlobalResorceStock globalResStock, TopResUiVew topResUI,
        BuildingList buildingList, EndGameScreen endGameScreen, TowerShotConfig towerShotConfig, 
        BuyItemScreenView buyItemScreenView, HireSystemView hireSystemView , EquipScreenView equipScreenView, 
        Camera camera, BaseCenterText centerText, TileUIView tileUIView, TileList tileList)
    {

        var tiles = GetTileList.GetTiles(gameConfig);
            
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, rightUI, btnConroller, btnParents, leftUI, bottonUI);
        var unitController = new UnitController();
        var buildController = new BuildGenerator(gameConfig, leftUI);
        var timeRemaining = new TimeRemainingController();
        var buildingResursesUIController = new BuildingResursesUIController(buildingsUI, /*buildingList,*/topResUI,globalResStock);
        var uiController = new UIController(leftUI, rightUI, bottonUI, centerUI);
        var towershotcontroller = new TowerShotController(towerShotConfig, levelGenerator, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, hireSystemView, levelGenerator);
        var buildingController = new BuildingController(centerText);
        
        var waveController = new WaveController(levelGenerator, rightUI, btnParents, gameConfig, centerText);
        var endGameController = new EndGameController(endGameScreen, levelGenerator);
        
        var globalResController = new MainResursesController(globalResStock, topResUI);
        //var buildController = new BuildGenerator(gameConfig, leftUI, layerMask, outPostSpawner);
        if (!gameConfig.ChangeVariant)
        {
            new ResourceGenerator(buildController.Buildings, gameConfig, levelGenerator);
        }
        else
        {
            new ResourceGenerator(buildController.Buildings, gameConfig, levelGenerator, 2);
        }
        var tilecontroller = new TileController(tileList, tileUIView, centerText, uiController, buildController, globalResStock, buildingController);
        var inputController = new InputController(tilecontroller, uiController);


        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildController);
        controller.Add(timeRemaining);
        controller.Add(unitController);
        controller.Add(inputController);
        controller.Add(buildingResursesUIController);
        controller.Add(globalResController);
        controller.Add(waveController);
        controller.Add(endGameController);
        controller.Add(towershotcontroller);
        controller.Add(hireSystemController);
        controller.Add(tilecontroller);
        // controller.Add(buildBuildingsUIController);


        var testDummyTargetController = new TestDummyTargetController(levelGenerator, gameConfig.TestBuilding);
        controller.Add(testDummyTargetController);
    }
}