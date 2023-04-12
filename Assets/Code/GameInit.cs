using Controllers;
using Controllers.BaseUnit;
using Controllers.OutPost;
using UnityEngine;
using ResurseSystem;
using BuildingSystem;
using Code.TowerShot;
using UnityEngine.AI;
using Views.BaseUnit.UI;
using Controllers.BuildBuildingsUI;
using CombatSystem;

public class GameInit
{
    public GameInit(Controller controller, GameConfig gameConfig, RightUI rightUI, NavMeshSurface navMeshSurface,
        Transform btnParents, LeftUI leftUI, LayerMask layerMask, UnitUISpawnerTest unitUISpawnerTest,
        BuildingsUI buildingsUI, GlobalResurseStock globalResStock, TopResUiVew topResUI,
        GlobalBuildingsModels globalBuildingmodel, EndGameScreen endGameScreen, TowerShotConfig towerShotConfig)
    {

        var tiles = GetTileList.GetTiles(gameConfig);
            
        var btnConroller = new BtnUIController(rightUI, gameConfig);
        var levelGenerator = new GeneratorLevelController(tiles, gameConfig, rightUI, btnConroller, btnParents, navMeshSurface);
        var unitController = new UnitController();
        var outPostSpawner = new OutpostSpawner(unitUISpawnerTest);
        var buildController = new BuildGenerator(gameConfig, leftUI, layerMask);
        var timeRemaining = new TimeRemainingController();
        var unitSpawner = new BaseUnitSpawner(gameConfig, unitController, outPostSpawner, gameConfig.BaseUnit);
        var buildingController = new BuildingResursesUIController(buildingsUI, globalBuildingmodel,topResUI,globalResStock);
        var inputController = new InputController(unitSpawner, buildingController, outPostSpawner);
        var towershotcontroller = new TowerShotController(towerShotConfig, levelGenerator, gameConfig.Bullet);
        
        var waveController = new WaveController(levelGenerator, gameConfig, rightUI, btnParents);
        var endGameController = new EndGameController(endGameScreen);
        
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


        controller.Add(btnConroller);
        controller.Add(levelGenerator);
        controller.Add(buildController);
        controller.Add(timeRemaining);
        controller.Add(unitController);
        controller.Add(outPostSpawner);
        controller.Add(unitSpawner);
        controller.Add(inputController);
        controller.Add(buildingController);
        controller.Add(globalResController);
        controller.Add(waveController);
        controller.Add(endGameController);
        controller.Add(towershotcontroller);

        var buildBuildingsUIController = new BuildBuildingsUIController(leftUI.BuildBuildingMenu,
            globalBuildingmodel, buildController);
        controller.Add(buildBuildingsUIController);

        var testDummyTargetController = new TestDummyTargetController(levelGenerator);
        controller.Add(testDummyTargetController);
    }
}