using BattleSystem;
using BattleSystem.MainTower;
using Code.GlobalGameState;
using Code.SceneConfigs;
using Configs;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UnitSystem;
using UnityEngine;

namespace Code.MVC_System
{
    public class BattleSceneGameInit
    {     
        public BattleSceneGameInit( 
            Controller controller, 
            ConfigsHolder configs, 
            Canvas canvas, 
            SceneObjectsHolder sceneObjectsHolder)
        {
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
                  
            var enemySpawnHandler 
                = new EnemySpawnHandler(enemySpawner, configs.EnemyWaveSettings, sceneObjectsHolder.BattleUI);
            var defendersSpawnHandler
                = new DefenderSpawnHandler(defendersSpawner, sceneObjectsHolder.BattleUI);

            var peaceStateManager = new PeaceStateManager();
            var battleStateManager 
                = new BattleStateManager(defendersSpawnHandler, enemySpawnHandler, mainTower, unitsContainer);
            
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager);
            

            controller.Add(timeRemainingService);
            controller.Add(unitsContainer);

            controller.Add(enemySpawner);
            controller.Add(defendersSpawner);

            controller.Add(mainTower);
            controller.Add(enemyBattleController);
            controller.Add(defenderBattleController);

            controller.Add(gameStateManager);
        }
    }
}


