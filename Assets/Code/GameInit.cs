using Controllers;
using Controllers.BaseUnit;
using Controllers.OutPost;
using UnityEngine;
using ResourceSystem;
using BuildingSystem;
using Code;
using Code.TileSystem;
using Code.TowerShot;
using Code.View;
using Views.BaseUnit.UI;
using Controllers.BuildBuildingsUI;
using CombatSystem;
using EquipmentSystem;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, RightUI rightUI, 
        Transform btnParents, LeftUI leftUI, BottonUI bottonUI,
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
        var buildingController = new BuildingResursesUIController(buildingsUI, /*buildingList,*/topResUI,globalResStock);
        var towershotcontroller = new TowerShotController(towerShotConfig, levelGenerator, gameConfig.Bullet);
        var eqScreenController = new EquipScreenController(equipScreenView, camera);
        var hireSystemController = new HireSystemController(globalResStock, buyItemScreenView, eqScreenController, hireSystemView, levelGenerator);
        
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
        var tilecontroller = new TileUIController(tileList, tileUIView, centerText, bottonUI.BuildingMenu, buildController, globalResStock);
        var inputController = new InputController(/*buildingController, */tilecontroller);


        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildController);
        controller.Add(timeRemaining);
        controller.Add(unitController);
        controller.Add(inputController);
        controller.Add(buildingController);
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