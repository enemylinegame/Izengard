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

            var warBuildingController = new WarBuildingsController(sceneObjectsHolder.MainTower, configs.MainTowerSettings);
            
            var targetFinder = new TargetFinder(warBuildingController);
            var navigationUpdater = new NavigationUpdater();
            navigationUpdater.AddNavigationSurface(sceneObjectsHolder.GroundSurface);

            var defendersSpawner = new DefendersSpawnController(configs.DefendersSpawnSettings.UnitsCreationData,
                GetPositions(sceneObjectsHolder.DefendersSpawnPoints));
            
            var battleController = new BattleUnitController(targetFinder);
            
            var unitSpawnObserver = new UnitSpawnObserver(enemySpawner, defendersSpawner,
                battleController);
            
            var enemySpawnLogic = new EnemySpawnLogicMock(enemySpawner);
            var defendersSpawnLogic = new DefendersSpawnLogicMock(defendersSpawner);

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


