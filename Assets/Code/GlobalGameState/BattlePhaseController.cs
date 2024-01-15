using BattleSystem;
using BattleSystem.MainTower;
using Code.SceneConfigs;
using Configs;
using GlobalGameState;
using SpawnSystem;
using System;
using Tools;
using UI;
using UnitSystem;
using UnityEngine;

namespace Code.GlobalGameState
{
    public class BattlePhaseController : IPhaseController, IOnUpdate, IOnFixedUpdate
    {
        private readonly BattleUIController _battleUIController;

        private readonly PauseController _pauseController;
        private readonly SpawnCreationController _spawnCreation;

        private readonly MainTowerController _mainTowerController;
        private readonly IUnitsContainer _unitsContainer;

        private readonly DefenderSpawnHandler _defendersSpawnHandler;
        private readonly EnemySpawnHandler _enemySpawnHandler;
        
        private readonly BaseBattleController _enemyBattleController;
        private readonly BaseBattleController _defenderBattleController;

        public event Action OnPhaseEnd;

        private bool _isBattleWork;

        public BattlePhaseController(
            SceneObjectsHolder sceneObjectsHolder,
            ConfigsHolder configs,
            PauseController pauseController,
            SpawnCreationController spawnCreation,
            MainTowerController mainTower,
            IUnitsContainer unitsContainer) 
        {
            _battleUIController = new BattleUIController(sceneObjectsHolder.BattleUI);

            _pauseController = pauseController;
            _spawnCreation = spawnCreation;

            _mainTowerController = mainTower;

            _unitsContainer = unitsContainer;

            var enemySpawner
             = new EnemySpawnController(sceneObjectsHolder.EnemySpawner, spawnCreation, unitsContainer);
            
            var defendersSpawner
                = new DefendersSpawnController(sceneObjectsHolder.DefendersSpawner, spawnCreation, unitsContainer);

            _enemySpawnHandler
                = new EnemySpawnHandler(enemySpawner, configs.EnemyWaveSettings, _battleUIController);
            _defendersSpawnHandler 
                = new DefenderSpawnHandler(defendersSpawner, _battleUIController);

            var targetFinder = new TargetFinder(mainTower, unitsContainer);

            _enemyBattleController 
                = new EnemyBattleController(configs.BattleSystemConst, targetFinder, unitsContainer, mainTower, enemySpawner);
            _defenderBattleController
                = new DefenderBattleController(configs.BattleSystemConst, targetFinder, unitsContainer, mainTower);

            _pauseController.Add(_enemyBattleController);
            _pauseController.Add(_defenderBattleController);

            _battleUIController.OnStartBattle += StartBattle;
            _battleUIController.OnPauseBattle += PauseBattle;
            _battleUIController.OnResetBattle += ResetBattle;

            _isBattleWork = false;
        }


        private void StartBattle()
        {
            _isBattleWork = true;
        }

        private void PauseBattle()
        {
            if (_isBattleWork)
            {
                _pauseController.Pause();
                _isBattleWork = false;
            }
            else
            {
                _pauseController.Release();
                _isBattleWork = true;
            }
        }

        private void ResetBattle()
        {
            _isBattleWork = false;
            
            _spawnCreation.ClearSpawnres();
            _unitsContainer.ClearData();

            _mainTowerController.Reset();
        }

        public void StartPhase()
        {
            _mainTowerController.OnMainTowerDestroyed += MainTowerWasDestroyed;
            _unitsContainer.OnAllEnemyDestroyed += AllEnemiesWasKilled;
        }


        public void EndPhase()
        {
            _mainTowerController.OnMainTowerDestroyed -= MainTowerWasDestroyed;
            _unitsContainer.OnAllEnemyDestroyed -= AllEnemiesWasKilled;

            OnPhaseEnd?.Invoke();
        }

        private void MainTowerWasDestroyed()
        {
            _enemySpawnHandler.StopWave(); 
            
            Debug.Log("You Lose!");

            EndPhase();
        }

        private void AllEnemiesWasKilled()
        {
            _enemySpawnHandler.StopWave();

            Debug.Log("You Win!");

            EndPhase();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isBattleWork == false)
                return;

            _enemyBattleController.OnUpdate(deltaTime);
            _defenderBattleController.OnUpdate(deltaTime);         
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_isBattleWork == false)
                return;

        }
    }
}