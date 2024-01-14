using BattleSystem;
using BattleSystem.MainTower;
using Code.GlobalGameState;
using Code.SceneConfigs;
using Configs;
using NewBuildingSystem;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UI;
using UnitSystem;
using UnityEngine;
using UserInputSystem;

namespace Code.MVC_System
{
    public class BattleSceneGameInit
    {

        private UserInput _userInput;

        public BattleSceneGameInit( 
            Controller controller, 
            ConfigsHolder configs, 
            Canvas canvas, 
            SceneObjectsHolder sceneObjectsHolder,
            GameObject spawnerPrefab,
            GameObject plane,
            Grid grid,
            Map map)
        {
            var userInputController = new UserInputController();
            _userInput = userInputController.UserInput;

            var timeRemainingService = new TimeRemainingController();
            var navigationUpdater = new NavigationUpdater();
            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);

            var rayCastController = new RayCastController(_userInput, configs.GameConfig);

            var battleUIController = new BattleUIController(sceneObjectsHolder.BattleUI);

            var mainTower 
                = new MainTowerController(sceneObjectsHolder.MainTower, configs.MainTowerSettings);

            var unitsContainer 
                = new UnitsContainer(
                    configs.BattleSystemConst, 
                    sceneObjectsHolder.BattleUI.UnitStatsPanel, 
                    rayCastController);
            
            var spawnerCreationController
                = new SpawnCreationController(
                    sceneObjectsHolder, spawnerPrefab, 
                    rayCastController, 
                    configs.ObjectsHolder, 
                    plane, grid);

            var battleStateManager 
                = new BattlePhaseController(
                    sceneObjectsHolder, 
                    configs, 
                    spawnerCreationController, 
                    mainTower, 
                    unitsContainer);

            var peaceStateManager = new PeacePhaseConttoller();
            
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager);

            var MapController = new MapController(map, configs.GameConfig.BattleStageMapSize);

            controller.Add(timeRemainingService);
            controller.Add(unitsContainer);
            controller.Add(rayCastController);

            controller.Add(spawnerCreationController);

            controller.Add(mainTower);

            controller.Add(gameStateManager);
        }
    }
}


