using System.Collections.Generic;
using BattleSystem;
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

            var enemySpawner = new EnemySpawnController( sceneObjectsHolder.EnemySpawnPoints, configs.EnemySpawnSettings);
            var targetFinder = new TargetFinder(sceneObjectsHolder.MainTower);
            var enemyBattleController = new EnemyBattleController(targetFinder);
            //enemySpawnController.OnUnitSpawned += OnCreatedUnit;
            var navigationUpdater = new NavigationUpdater();
            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);
            
            
                
            controller.Add(timeRemainingService);
            controller.Add(enemySpawner);
            controller.Add(enemyBattleController);
        }
    }
}


