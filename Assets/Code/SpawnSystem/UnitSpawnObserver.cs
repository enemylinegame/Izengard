using BattleSystem;
using UnitSystem;

namespace SpawnSystem
{
    public class UnitSpawnObserver
    {

        private readonly EnemySpawnController _enemySpawner;
        private readonly DefendersSpawnController _defendersSpawner;
        //private readonly EnemyBattleController _enemyBattleController;
        //private readonly DefenderBattleController _defenderBattleController;
        private readonly FifthBattleController _battleController;


        public UnitSpawnObserver(EnemySpawnController enemySpawner, 
            DefendersSpawnController defendersSpawner, 
            //EnemyBattleController enemyBattleController, 
            //DefenderBattleController defenderBattleController,
            FifthBattleController fifthBattleController
            )
        {
            _enemySpawner = enemySpawner;
            _defendersSpawner = defendersSpawner;
            //_enemyBattleController = enemyBattleController;
            //_defenderBattleController = defenderBattleController;
            _battleController = fifthBattleController;
            _enemySpawner.OnUnitSpawned += EnemySpawned;
            _defendersSpawner.OnUnitSpawned += DefenderSpawned;
        }

        private void EnemySpawned(IUnit enemyUnit)
        {
            enemyUnit.Enable();
            //_enemyBattleController.AddUnit(enemyUnit);
            //_defenderBattleController.AddUnit(enemyUnit);
            _battleController.AddUnit(enemyUnit);
        }

        private void DefenderSpawned(IUnit defenderUnit)
        {
            defenderUnit.Enable();
            //_enemyBattleController.AddUnit(defenderUnit);
            //_defenderBattleController.AddUnit(defenderUnit);
            _battleController.AddUnit(defenderUnit);
        }
        
        
        
    }
}