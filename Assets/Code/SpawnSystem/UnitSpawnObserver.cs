using BattleSystem;
using UnitSystem;

namespace SpawnSystem
{
    public class UnitSpawnObserver
    {

        private readonly EnemySpawnController _enemySpawner;
        private readonly DefendersSpawnController _defendersSpawner;
        private readonly EnemyBattleController _enemyBattleController;
        private readonly DefenderBattleController _defenderBattleController;


        public UnitSpawnObserver(EnemySpawnController enemySpawner, 
            DefendersSpawnController defendersSpawner, 
            EnemyBattleController enemyBattleController, 
            DefenderBattleController defenderBattleController)
        {
            _enemySpawner = enemySpawner;
            _defendersSpawner = defendersSpawner;
            _enemyBattleController = enemyBattleController;
            _defenderBattleController = defenderBattleController;
            
            _enemySpawner.OnUnitSpawned += EnemySpawned;
            _defendersSpawner.OnUnitSpawned += DefenderSpawned;
        }

        private void EnemySpawned(IUnit enemyUnit)
        {
            enemyUnit.Enable();
            _enemyBattleController.AddUnit(enemyUnit);
            _defenderBattleController.AddUnit(enemyUnit);
        }

        private void DefenderSpawned(IUnit defenderUnit)
        {
            _enemyBattleController.AddUnit(defenderUnit);
            _defenderBattleController.AddUnit(defenderUnit);
        }
        
        
        
    }
}