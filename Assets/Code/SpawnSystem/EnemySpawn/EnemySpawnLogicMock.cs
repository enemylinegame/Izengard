using SpawnSystem;
using Tools;
using UnitSystem.Enum;

namespace BattleSystem
{
    public class EnemySpawnLogicMock
    {
        
        private readonly EnemySpawnController _spawnController;
        private TimeRemaining _timer;
        
        private bool _isTiming;
        
        public EnemySpawnLogicMock(EnemySpawnController enemySpawnController)
        {
            _spawnController = enemySpawnController;
            _timer = new TimeRemaining(SpawnPack, 1.0f);
        }


        public void StartSpawn()
        {
            if (!_isTiming)
            {
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

        private void SpawnPack()
        {
            _isTiming = false;
            _spawnController.SpawnUnit(UnitRoleType.Imp);
            _spawnController.SpawnUnit(UnitRoleType.Imp);
            _spawnController.SpawnUnit(UnitRoleType.Hound);
        }
        
    }
}