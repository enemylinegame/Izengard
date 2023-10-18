using Tools;
using UnitSystem.Enum;

namespace BattleSystem
{
    public class DefendersSpawnLogicMock
    {

        private readonly DefendersSpawnController _spawnController;
        private TimeRemaining _timer;
        
        private bool _isTiming;

        public DefendersSpawnLogicMock(DefendersSpawnController defendersSpawnController)
        {
            _spawnController = defendersSpawnController;
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
            _spawnController.SpawnUnit(UnitRoleType.Militiaman);
            _spawnController.SpawnUnit(UnitRoleType.Militiaman);
        }
        
        
    }
}