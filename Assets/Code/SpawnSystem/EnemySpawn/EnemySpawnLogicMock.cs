using SpawnSystem;
using Tools;
using UnitSystem.Enum;

namespace BattleSystem
{
    public class EnemySpawnLogicMock
    {
        
        private readonly EnemySpawnController _spawnController;
        private TimeRemaining _timer;

        private int _stepCounter;
        private int _stepsTotal;
        
        private bool _isTiming;
        
        public EnemySpawnLogicMock(EnemySpawnController enemySpawnController)
        {
            _spawnController = enemySpawnController;
            _timer = new TimeRemaining(SpawnStep, 4.0f);
        }


        public void StartSpawn()
        {
            if (!_isTiming)
            {
                _stepsTotal = 7;
                _stepCounter = 0;
                TimersHolder.AddTimer(_timer);
                _isTiming = true;
            }
        }

        public void StopSpawn()
        {
            if (_isTiming)
            {
                TimersHolder.RemoveTimer(_timer);
                _isTiming = false;
            }
        }

        private void SpawnStep()
        {
            _isTiming = false;
            _stepCounter++;
            SpawnPack();
            if (_stepCounter < _stepsTotal)
            {
                TimersHolder.AddTimer(_timer);
                _isTiming = true;
            }
        }

        private void SpawnPack()
        {
            _spawnController.SpawnUnit(UnitType.Imp);
            _spawnController.SpawnUnit(UnitType.Imp);
            _spawnController.SpawnUnit(UnitType.Hound);
        }
        
    }
}