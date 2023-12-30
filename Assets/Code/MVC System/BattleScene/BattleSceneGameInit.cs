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
                       
            var mainTower 
                = new MainTowerController(sceneObjectsHolder.MainTower, configs.MainTowerSettings);

            var unitsContainer = new UnitsContainer(configs.BattleSystemConst);
            var targetFinder = new TargetFinder(mainTower, unitsContainer);

            var enemySpawner 
                = new EnemySpawnController(sceneObjectsHolder.EnemySpawner, unitsContainer);
            var defendersSpawner 
                = new DefendersSpawnController(sceneObjectsHolder.DefendersSpawner, unitsContainer);
            
            var enemyBattleController 
                = new EnemyBattleController(configs.BattleSystemConst, targetFinder, unitsContainer, mainTower, enemySpawner);
            var defenderBattleController 
                = new DefenderBattleController(configs.BattleSystemConst, targetFinder, unitsContainer, mainTower);
            
            var rayCastController = new RayCastController(_userInput, configs.GameConfig);

            var battleUIController = new BattleUIController(sceneObjectsHolder.BattleUI);

            var spawnerCreationController
                = new SpawnCreationController(
                    sceneObjectsHolder.BattleUI.SpawnPanel,
                    sceneObjectsHolder.BattleUI.SpawnerTypeSelection,
                    spawnerPrefab,
                    rayCastController,
                    configs.ObjectsHolder,
                    plane, grid);

            var enemySpawnHandler 
                = new EnemySpawnHandler(enemySpawner, configs.EnemyWaveSettings, battleUIController);
            var defendersSpawnHandler
                = new DefenderSpawnHandler(defendersSpawner, battleUIController);

            var peaceStateManager = new PeacePhaseConttoller();
            var battleStateManager 
                = new BattlePhaseController(defendersSpawnHandler, enemySpawnHandler, mainTower, unitsContainer);
            
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager);

            var MapController = new MapController(map, configs.GameConfig.BattleStageMapSize);

            controller.Add(timeRemainingService);
            controller.Add(unitsContainer);
            controller.Add(rayCastController);

            controller.Add(spawnerCreationController);
            controller.Add(enemySpawner);
            controller.Add(defendersSpawner);

            controller.Add(mainTower);
            controller.Add(enemyBattleController);
            controller.Add(defenderBattleController);

            controller.Add(gameStateManager);
        }
    }
}


