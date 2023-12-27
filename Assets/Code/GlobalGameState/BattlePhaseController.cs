using BattleSystem.MainTower;
using GlobalGameState;
using SpawnSystem;
using System;
using UnitSystem;
using UnityEngine;

namespace Code.GlobalGameState
{
    public class BattlePhaseController : IPhaseController
    {
        private readonly DefenderSpawnHandler _defendersSpawnHandler;
        private readonly EnemySpawnHandler _enemySpawnHandler;
        private readonly IUnitsContainer _unitsContainer;
        private readonly MainTowerController _mainTower;

        public event Action OnPhaseEnd;

        public BattlePhaseController(
            DefenderSpawnHandler defendersSpawnHandler,
            EnemySpawnHandler enemySpawnHandler,
            MainTowerController mainTower,
            IUnitsContainer unitsContainer)
        {
            _defendersSpawnHandler = defendersSpawnHandler;
            _enemySpawnHandler = enemySpawnHandler;
            _mainTower = mainTower;
            _unitsContainer = unitsContainer;
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
    }
}