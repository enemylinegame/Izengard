using System.Collections.Generic;
using BattleSystem;
using Code.GlobalGameState;
using Code.SceneConfigs;
using Configs;
using SpawnSystem;
using Tools;
using Tools.Navigation;
using UnityEngine;

namespace Code.MVC_System.Mocks
{
    public class GameInitMock
    {
        
        public GameInitMock( 
            Controller controller, 
            ConfigsHolder configs, 
            AudioSource clickAudioSource, 
            Canvas canvas, 
            SceneObjectsHolder sceneObjectsHolder)
        {
            var timeRemainingService = new TimeRemainingController();

            var enemySpawner = new EnemySpawnController(sceneObjectsHolder.EnemySpawner);
            var targetFinder = new TargetFinder(sceneObjectsHolder.MainTower);
            var enemyBattleController = new EnemyBattleController(targetFinder);
            var navigationUpdater = new NavigationUpdater();
            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);

            var defendersSpawner = new DefendersSpawnController();
            var defendersBattleController = new DefenderBattleController(targetFinder);
            
            var unitSpawnObserver = new UnitSpawnObserver(enemySpawner, defendersSpawner, 
                enemyBattleController, defendersBattleController);

            var enemySpawnLogic = new EnemySpawnLogicMock(enemySpawner);
            var defendersSpawnLogic = new DefendersSpawnLogicMock(defendersSpawner);

            var peaceStateManager = new PeaceStateManager();
            var battleStateManager = new BattleStateManager(defendersSpawnLogic, enemySpawnLogic);
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager); 
            
            
            controller.Add(timeRemainingService);
            controller.Add(enemySpawner);
            controller.Add(enemyBattleController);
            controller.Add(defendersBattleController);
            controller.Add(gameStateManager);
        }
    }
}


