﻿using BattleSystem;
using BattleSystem.MainTower;
using Code.SceneConfigs;
using Configs;
using GlobalGameState;
using SpawnSystem;
using System;
using UI;
using UnitSystem;
using UnityEngine;

namespace Code.GlobalGameState
{
    public class BattlePhaseController : IPhaseController, IOnUpdate, IOnFixedUpdate
    {
        private readonly BattleUIController _battleUIController;
        private readonly MainTowerController _mainTower;
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
            SpawnCreationController spawnCreation,
            MainTowerController mainTower,
            IUnitsContainer unitsContainer) 
        {
            _battleUIController = new BattleUIController(sceneObjectsHolder.BattleUI);

            _mainTower = mainTower;

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
           
        }

        private void ResetBattle()
        {
            _isBattleWork = false;
        }

        public void StartPhase()
        {
            _mainTower.OnMainTowerDestroyed += MainTowerWasDestroyed;
            _unitsContainer.OnAllEnemyDestroyed += AllEnemiesWasKilled;
        }


        public void EndPhase()
        {
            _mainTower.OnMainTowerDestroyed -= MainTowerWasDestroyed;
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