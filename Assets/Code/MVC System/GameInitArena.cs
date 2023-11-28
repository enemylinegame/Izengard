using System.Collections.Generic;
using BattleSystem;
using BattleSystem.Buildings;
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
    public class GameInitArena
    {
        
        public GameInitArena( 
            Controller controller, 
            ConfigsHolder configs, 
            AudioSource clickAudioSource, 
            Canvas canvas, 
            SceneObjectsHolder sceneObjectsHolder)
        {
            var timeRemainingService = new TimeRemainingController();

            var enemySpawner = new EnemySpawnController(sceneObjectsHolder.EnemySpawner);

            var warBuildingController = new WarBuildingsController(sceneObjectsHolder.MainTower, configs.MainTowerSettings);

            var unitsContainer = new UnitsContainer();
            var targetFinder = new TargetFinder(warBuildingController, unitsContainer);
            var navigationUpdater = new NavigationUpdater();

            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);

            var defendersSpawner = new DefendersSpawnController(sceneObjectsHolder.DefendersSpawner);
            
            var battleController = new DefenderBattleController(configs.BattleSystemConst, targetFinder, unitsContainer);
                  
            var enemySpawnLogic = new EnemySpawnHandler(enemySpawner, configs.EnemyWaveSettings);
            var defendersSpawnLogic = new DefenderSpawnHandler(defendersSpawner, sceneObjectsHolder.DefenderSpawnButton);

            var peaceStateManager = new PeaceStateManager();
            var battleStateManager = new BattleStateManager(defendersSpawnLogic, enemySpawnLogic);
            var gameStateManager = new GameStateManager(peaceStateManager, battleStateManager); 
            
            
            controller.Add(timeRemainingService);
            controller.Add(enemySpawner);
            controller.Add(gameStateManager);
            controller.Add(battleController);
            controller.Add(warBuildingController);
            
        }

        private List<Vector3> GetPositions(List<Transform> transforms)
        {
            List<Vector3> positions = new();

            foreach (var transform in transforms)
            {
                positions.Add(transform.position);
            }
            
            return positions;
        }

    }
}


