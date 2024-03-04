using BattleSystem.MainTower;
using Code.GlobalGameState;
using Code.SceneConfigs;
using Configs;
using NewBuildingSystem;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UI;
using UnityEngine;
using UserInputSystem;

namespace Code.MVC_System
{
    public class BattleSceneGameInit
    {
        private readonly UserInput _userInput;

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

            var uIPanelInit = new BattleUIPanelInitialization(configs.UIElementsConfig, canvas);

            var timeRemainingService = new TimeRemainingController();
            var navigationUpdater = new NavigationUpdater();
            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);


            var pasueController = new PauseController();

            var rayCastController = new RayCastController(_userInput, configs.GameConfig);

            var mainTower 
                = new MainTowerController(
                    uIPanelInit.BattleUIController,
                    sceneObjectsHolder,
                    configs.MainTowerSettings, 
                    rayCastController);
            
            var spawnerCreationController
                = new SpawnCreationController(
                    uIPanelInit.BattleUIController,
                    sceneObjectsHolder, spawnerPrefab, 
                    rayCastController, 
                    configs.ObjectsHolder, 
                    plane, grid);

            var battleStateManager 
                = new BattlePhaseController(
                    uIPanelInit.BattleUIController,
                    sceneObjectsHolder, 
                    configs, 
                    rayCastController,
                    pasueController,
                    spawnerCreationController, 
                    mainTower);

            var peaceStateManager = new PeacePhaseConttoller();
            
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager);

            var MapController = new MapController(map, configs.GameConfig.BattleStageMapSize);


            controller.Add(timeRemainingService);
            controller.Add(pasueController);

            controller.Add(rayCastController);

            controller.Add(spawnerCreationController);

            controller.Add(mainTower);

            controller.Add(gameStateManager);
        }
    }
}


