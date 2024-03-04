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
using UserInputSystem;

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

        private BattlePhaseState _state;

        public BattlePhaseController(
            BattleUIController battleUIController,
            SceneObjectsHolder sceneObjectsHolder, 
            ConfigsHolder configs, 
            RayCastController rayCastController,
            PauseController pauseController, 
            SpawnCreationController spawnCreation, 
            MainTowerController mainTower)
        {
            _battleUIController = battleUIController;

            _pauseController = pauseController;
            _spawnCreation = spawnCreation;

            _mainTowerController = mainTower;

            _unitsContainer = new UnitsContainer(configs.BattleSystemConst, _battleUIController.View.UnitStatsPanel, rayCastController); ;

            var enemySpawner
             = new EnemySpawnController(sceneObjectsHolder.EnemySpawner, spawnCreation, _unitsContainer);

            var defendersSpawner
                = new DefendersSpawnController(sceneObjectsHolder.DefendersSpawner, spawnCreation, _unitsContainer);

            _enemySpawnHandler
                = new EnemySpawnHandler(enemySpawner, configs.EnemyWaveSettings, _battleUIController);
            _defendersSpawnHandler
                = new DefenderSpawnHandler(defendersSpawner, _battleUIController);

            var targetFinder = new TargetFinder(mainTower, _unitsContainer);

            _enemyBattleController
                = new EnemyBattleController(configs.BattleSystemConst, targetFinder, _unitsContainer, mainTower, enemySpawner);
            _defenderBattleController
                = new DefenderBattleController(configs.BattleSystemConst, targetFinder, _unitsContainer, mainTower);
            
            _pauseController.Add(_unitsContainer);
            _pauseController.Add(_enemyBattleController);
            _pauseController.Add(_defenderBattleController);

            _battleUIController.OnStartBattle += StartBattle;
            _battleUIController.OnPauseBattle += PauseBattle;
            _battleUIController.OnResumeBattle += ResumeBattle;
            _battleUIController.OnResetBattle += ResetBattle;

            _state = BattlePhaseState.None;
        }

     

        private void StartBattle()
        {
            if (_unitsContainer.DefenderUnits.Count == 0
                && _unitsContainer.EnemyUnits.Count == 0) 
            {
                return;
            }

            _state = BattlePhaseState.Proceed;
            _battleUIController.BlockStartButton(true);
        }

        private void PauseBattle()
        {
            if (_state != BattlePhaseState.Proceed)
                return;

            _battleUIController.SwitchPauseUI(false);

            _pauseController.Pause();

            _state = BattlePhaseState.Pause;
        }

        private void ResumeBattle()
        {
            if (_state != BattlePhaseState.Pause)
                return;

            _battleUIController.SwitchPauseUI(true);

            _pauseController.Release();

            _state = BattlePhaseState.Proceed;
        }

        private void ResetBattle()
        {
            _spawnCreation.Reset();
            _unitsContainer.ClearData();

            _mainTowerController.Reset();

            ResumeBattle();

            StartPhase();
        }

        public void StartPhase()
        {
            _mainTowerController.OnMainTowerDestroyed += MainTowerWasDestroyed;
            _unitsContainer.OnAllEnemyDestroyed += AllEnemiesWasKilled;

            _battleUIController.BlockStartButton(false);

            _state = BattlePhaseState.Start;
        }


        public void EndPhase()
        {
            _mainTowerController.OnMainTowerDestroyed -= MainTowerWasDestroyed;
            _unitsContainer.OnAllEnemyDestroyed -= AllEnemiesWasKilled;

            _state = BattlePhaseState.End;

            OnPhaseEnd?.Invoke();
        }

        private void MainTowerWasDestroyed()
        {
            _enemySpawnHandler.StopWave();

            DebugGameManager.Log("You Lose!", new[] { DebugTags.System });

            EndPhase();
        }

        private void AllEnemiesWasKilled()
        {
            _enemySpawnHandler.StopWave();

            DebugGameManager.Log("You Win!", new[] { DebugTags.System });

            EndPhase();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_state != BattlePhaseState.Proceed)
                return;

            _unitsContainer.OnUpdate(deltaTime);
            _enemyBattleController.OnUpdate(deltaTime);
            _defenderBattleController.OnUpdate(deltaTime);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_state != BattlePhaseState.Proceed)
                return;

        }
    }
}