using BattleSystem;
using UnitSystem;

namespace SpawnSystem
{
    public class UnitSpawnObserver
    {

        private readonly EnemySpawnController _enemySpawner;
        private readonly DefendersSpawnController _defendersSpawner;
        private readonly UnitBattleController _battleController;


        public UnitSpawnObserver(
            EnemySpawnController enemySpawner,
            DefendersSpawnController defendersSpawner, 
            UnitBattleController battleController)
        {
            _enemySpawner = enemySpawner;
            _defendersSpawner = defendersSpawner;
            _battleController = battleController;
            _enemySpawner.OnUnitSpawned += EnemySpawned;
            _defendersSpawner.OnUnitSpawned += DefenderSpawned;
        }

        private void EnemySpawned(IUnit enemyUnit)
        {
            enemyUnit.Enable();
           // _battleController.AddUnit(enemyUnit);
        }

        private void DefenderSpawned(IUnit defenderUnit)
        {
            defenderUnit.Enable();
            //_battleController.AddUnit(defenderUnit);
        }

    }
}